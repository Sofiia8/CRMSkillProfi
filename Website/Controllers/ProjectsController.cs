using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Website.Data;
using Website.Models;

namespace Website.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly IRepositoryProjects _repositoryProjects;
        private readonly IRepositorySettings _repositorySettings;
        public ProjectsController(IRepositoryProjects repositoryProjects, IRepositorySettings repositorySettings)
        {
            _repositoryProjects = repositoryProjects;
            _repositorySettings = repositorySettings;
        }
        public async Task<IActionResult> Projects()
        {
            var result = await GetProjects();
            if (result == null)
            {
                return RedirectToAction("NotSuccess", "Auth", new { errors = "Нет соединения с сервером" });
            }
            return View(result);
        }
        public async Task<ProjectsViewModel> GetProjects()
        {
            var settings = await _repositorySettings.GetMainSettings();
            if (settings == null)
            {
                return null;
            }
            var projSetView = new ProjectsViewModel();
            var fields = typeof(ProjectsViewModel).GetProperties();
            foreach (var field in fields)
            {
                if (field.Name == "Projects")
                    continue;
                field?.SetValue(projSetView, settings[field.Name]);
            }

            var allItems = await _repositoryProjects.GetItems();
            if (allItems == null)
            {
                return null;
            }
            projSetView.Projects = allItems;
            return projSetView;
        }
        public async Task<IActionResult> AdminProjects()
        {
            var result = await GetProjects();
            if (result == null)
            {
                return RedirectToAction("NotSuccess", "Auth", new { errors = "Нет соединения с сервером" });
            }
            return View(result);
        }
        public async Task<IActionResult> GetItemDescriptionId(int id)
        {
            var settings = await _repositorySettings.GetMainSettings();
            if (settings == null)
            {
                return RedirectToAction("NotSuccess", "Auth", new { errors = "Нет соединения с сервером" });
            }
            var projSetView = new ProjectSettingViewModel();
            var fields = typeof(ProjectSettingViewModel).GetProperties();
            foreach (var field in fields)
            {
                if (field.Name == "Project_")
                    continue;
                field?.SetValue(projSetView, settings[field.Name]);
            }

            var item = await _repositoryProjects.GetItemById(id);
            if(item == null)
            {
                return RedirectToAction("NotSuccess", "Auth", new { errors = "Сервис не доступен." });
            }
            projSetView.Project_ = item;
            return View("ProjectPage", projSetView);
        }
        [HttpGet]
        public async Task<IActionResult> AddNewItem()
        {
            string? jwt = Request.Cookies["jwt"];
            if (jwt == null) return Redirect("/Auth/Login");    

            Project project = new Project();

            return View(project);
        }

        [HttpPost]
        public async Task<IActionResult> AddNewItem(Project project)
        {
            string? jwt = Request.Cookies["jwt"];
            if (jwt == null) return Redirect("/Auth/Login");

            HttpStatusCode httpstatus = await _repositoryProjects.AddNewProject(project, jwt);
            if (httpstatus != HttpStatusCode.Created && httpstatus != HttpStatusCode.OK)
            {
                return RedirectToAction("NotSuccess", "Auth", new { errors = "Не удалось создать проект" });
            }
            return Redirect("/Projects/AdminProjects");
        }
        
        [HttpPost]
        public async Task<IActionResult> AddImageForProject(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                ModelState.AddModelError("imageFile", "Please upload an image.");
                return BadRequest(ModelState);
            }

            var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "assets", "img");

            if (!Directory.Exists(uploadsPath))
            {
                Directory.CreateDirectory(uploadsPath);
            }

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
            var filePath = Path.Combine(uploadsPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }
            
            return Ok(fileName); 
        }

        [HttpGet]
        public async Task<IActionResult> EditItemId (int id)
        {
            string? jwt = Request.Cookies["jwt"];
            if (jwt == null) return Redirect("/Auth/Login");

            var project = await _repositoryProjects.GetItemById(id);
            if (project == null)
                return RedirectToAction("NotSuccess", "Auth", new { errors = "Не удается открыть редактируемый элемент" });

            return View(project);
        }

        [HttpPost]
        public async Task<IActionResult> EditItemId(int id, Project project)
        {
            string? jwt = Request.Cookies["jwt"];
            if (jwt == null) return Redirect("/Auth/Login");

            HttpStatusCode httpstatus = await _repositoryProjects.EditProject(id, project, jwt);
            if (httpstatus != HttpStatusCode.NoContent && httpstatus != HttpStatusCode.OK)
            {
                return RedirectToAction("NotSuccess", "Auth", new { errors = "Не удалось отредактировать проект" });
            }
            return Redirect("/Projects/AdminProjects");
        }

        [HttpGet]
        public async Task<IActionResult> DeleteItemId(int id)
        {
            string? jwt = Request.Cookies["jwt"];
            if (jwt == null) return Redirect("/Auth/Login");

            var project = await _repositoryProjects.GetItemById(id);
            if (project == null)
                return RedirectToAction("NotSuccess", "Auth", new { errors = "Не удается открыть редактируемый элемент" });

            return View(project);
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmDeleteItem(int id)
        {
            string? jwt = Request.Cookies["jwt"];
            if (jwt == null) return Redirect("/Auth/Login");

            HttpStatusCode httpstatus = await _repositoryProjects.DeleteProject(id, jwt);
            if (httpstatus != HttpStatusCode.NoContent && httpstatus != HttpStatusCode.OK)
            {
                return RedirectToAction("NotSuccess", "Auth", new { errors = "Не удалось удалить проект" });
            }
            return Redirect("/Projects/AdminProjects");
        }
    }
}
