using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Website.Models;

namespace Website.Data
{
    public interface IRepositoryProjects
    {
        Task<IEnumerable<Project>> GetItems();
        Task<Project> GetItemById(int id);
        Task<HttpStatusCode> AddNewProject(Project project, string jwt);
        Task<HttpStatusCode> EditProject(int id, Project project, string jwt);
        Task<HttpStatusCode> DeleteProject(int id, string jwt);
    }
}
