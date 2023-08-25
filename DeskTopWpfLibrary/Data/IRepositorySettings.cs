using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using DeskTopWpfLibrary.Models;

namespace DeskTopWpfLibrary.Data
{
    public interface IRepositorySettings
    {
        Task<Dictionary<string, string>> GetMainSettings();
        Task<MainSettings> GetMainSettingByCode(int code, string jwt);
        Task<HttpStatusCode> EditMainSettingByCode(int id, string newValue, string jwt);
    }
}
