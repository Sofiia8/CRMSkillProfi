using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Threading.Tasks;
using Website.Data;
using Website.Models;

namespace Website.Controllers
{
    public class AuthController : Controller
    {
        public static HttpClient httpClient;
        private readonly IRepositorySettings _repositorySettings;

        public AuthController(IRepositorySettings repositorySettings)
        {
            _repositorySettings = repositorySettings;
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        [HttpGet]
        public async Task<IActionResult> Login()
        {
            var settings = await _repositorySettings.GetMainSettings();
            if (settings == null)
            {
                return RedirectToAction("NotSuccess", "Auth", new { errors = "Нет соединения с сервером" });
            }
            var logSetView = new LoginSettingViewModel();
            var fields = typeof(LoginSettingViewModel).GetProperties();
            foreach (var field in fields)
            {
                if (field.Name == "Login" || field.Name == "Password")
                    continue;
                field?.SetValue(logSetView, settings[field.Name]);
            }
            return View(logSetView);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginSettingViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var response = await httpClient.PostAsync("https://localhost:44376/api/user/login",
                                            JsonContent.Create(loginViewModel, typeof(LoginSettingViewModel)));
                    if (!response.IsSuccessStatusCode)
                    {
                        string text = await response.Content.ReadAsStringAsync();
                        return RedirectToAction("NotSuccess", "Auth", new { errors = text.Trim('"') });
                    }

                    string json = await response.Content.ReadAsStringAsync();
                    JObject jo = JObject.Parse(json);
                    string jwt = jo["token"].ToString();

                    Response.Cookies.Append("jwt", jwt, new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Strict
                    });

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, jo["userName"].ToString())
                    };
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var authProperties = new AuthenticationProperties();

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                                                        new ClaimsPrincipal(claimsIdentity), authProperties);
                    return RedirectToAction("Create", "Request");
                }

                catch(Exception ex)
                {
                    return RedirectToAction("NotSuccess", "Auth", new { errors = "Сервис не доступен" });
                }
            }
            return View(loginViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            Response.Cookies.Delete("jwt");
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Create", "Request");
        }

        public async Task<IActionResult> NotSuccess(string errors)
        {
            ViewBag.errors = errors;
            return View();
        }

    }
}
