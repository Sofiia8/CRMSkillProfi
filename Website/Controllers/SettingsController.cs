using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;
using Website.Data;
using Website.Models;

namespace Website.Controllers
{
    public class SettingsController : Controller
    {
        private readonly IRepositorySettings _repositorySettings;
        public SettingsController(IRepositorySettings repository)
        {
            _repositorySettings = repository;
        }

        public async Task<IActionResult> SettingsAdmin()
        {
            var settings = await _repositorySettings.GetMainSettings();
            if (settings == null)
            {
                return RedirectToAction("NotSuccess", "Account", new { errors = "Нет соединения с сервером" });
            }
            var appSetView = new AppSettingView();
            var fields = typeof(AppSettingView).GetProperties();

            foreach (var field in fields)
            {
                field?.SetValue(appSetView, settings[field.Name]);
            }
            return View(appSetView);
        }

        [HttpGet]
        public async Task<IActionResult> ChangeSetting(int code)
        {
            string? jwt = Request.Cookies["jwt"];
            if (jwt == null) return Redirect("/Auth/Login");

            var setting = await _repositorySettings.GetMainSettingByCode(code, jwt);
            if (setting == null)
            {
                return RedirectToAction("NotSuccess", "Auth", new { errors = "Нет соединения с сервером" });
            }

            return View("ChangeSettingForm", new ChangeFormNewModel { id = setting.Id, currentName = setting.valueField });
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> ChangeSetting(ChangeFormNewModel model)
        {
            string? jwt = Request.Cookies["jwt"];
            if (jwt == null) return Redirect("/Auth/Login");
            if (model.newName == null)
                return RedirectToAction("NotSuccess", "Auth", new { errors = "Не удалось применить изменения на сервере. Значение не может быть пустым." });

            var result = await _repositorySettings.EditMainSettingByCode(model.id, model.newName, jwt);
            if (result != HttpStatusCode.OK)
            {
                return RedirectToAction("NotSuccess", "Auth", new { errors = "Не удалось применить изменения на сервере" });
            }

            return RedirectToAction("SettingsAdmin");
        }
    }
}
