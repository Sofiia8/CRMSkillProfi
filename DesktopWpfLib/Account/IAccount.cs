using DesktopWpfLib.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DesktopWpfLib.Account
{
    public interface IAccount
    {
        string Token { get; }
        string UserName { get; }
        bool Authorized { get; set; }
        Task<string[]> Login(LoginModel model);
        void Logout();
    }
}
