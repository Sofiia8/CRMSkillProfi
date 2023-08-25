using DeskTopWpfLibrary.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DeskTopWpfLibrary.Account
{
    public interface IAccount
    {
        string Token { get; }
        string UserName { get; }
        bool Authorized { get; set; }
        Task<string[]> Login(LoginSettingViewModel model);
        void Logout();
    }
}
