using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using DesktopWpfLib.Models;

namespace DesktopWpfLib.Data
{
    public interface IRepositorySettings
    {
        Task<Dictionary<string, string>> GetMainSettings();
        Task<MainSettings> GetMainSettingByCode(int code, string jwt);
        Task<HttpStatusCode> EditMainSettingByCode(int id, string newValue, string jwt);
    }
}
