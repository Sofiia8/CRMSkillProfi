using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using DeskTopWpfLibrary.Models;
using DeskTopWpfLibrary.Data;

namespace Website.Data
{
    public class RepositorySettingsApi : IRepositorySettings
    {
        public static HttpClient httpClient;
        public RepositorySettingsApi()
        {
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<Dictionary<string, string>> GetMainSettings()
        {
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get,
                        $"https://localhost:44376/api/MainSettings");

            try
            {
                var response = await httpClient.SendAsync(httpRequestMessage);
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }
                string result = await response.Content.ReadAsStringAsync();
                JToken jt = JToken.Parse(result);
                var mainSettings = jt.ToObject<IEnumerable<MainSettings>>();
                Dictionary<string, string> dict = new Dictionary<string, string>();
                foreach( var setting in mainSettings)
                {
                    dict.Add(setting.descriptionField, setting.valueField);
                }
                return dict;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<MainSettings> GetMainSettingByCode(int code, string jwt)
        {
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get,
                        $"https://localhost:44376/api/MainSettings/GetMainSettingByCode/{code}");
            httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

            try
            {
                var response = await httpClient.SendAsync(httpRequestMessage);
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }
                string result = await response.Content.ReadAsStringAsync();
                JObject jo = JObject.Parse(result);
                return jo.ToObject<MainSettings>();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<HttpStatusCode> EditMainSettingByCode(int id, string newValue, string jwt)
        {
            string escaped = Uri.EscapeDataString(newValue);
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Put,
                        $"https://localhost:44376/api/MainSettings/PutMainSettingByCode/{id}/{escaped}");

            httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

            try 
            {
                var response = await httpClient.SendAsync(httpRequestMessage);
                return response.StatusCode;
            }

            catch (Exception ex)
            {
                return HttpStatusCode.ServiceUnavailable;
            }
        }
    }
}
