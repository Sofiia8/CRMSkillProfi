﻿using DesktopWpfLib.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace DesktopWpfLib.Data
{
    public class RepositoryApi: IRepository<ApplicationView>
    {
        public static HttpClient httpClient;
        public RepositoryApi()
        {
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }


        public async Task<ObservableCollection<ApplicationView>> GetItems(string jwt)
        {
            HttpRequestMessage httpRequestMessage =
                    new HttpRequestMessage(HttpMethod.Get, "https://localhost:44376/api/Application");
            httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

            try
            {
                var response = await httpClient.SendAsync(httpRequestMessage);
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }
                string result = await response.Content.ReadAsStringAsync();
                JToken jt = JToken.Parse(result);
                return jt.ToObject<ObservableCollection<ApplicationView>>();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<ObservableCollection<ApplicationView>> GetItemsFromToDates(DateTime dateTimeFrom, DateTime dateTimeTo, string jwt)
        {
            HttpRequestMessage httpRequestMessage =
                        new HttpRequestMessage(HttpMethod.Get, $"https://localhost:44376/api/Application/GetFromToDates?" +
                        $"dateTimeFrom={dateTimeFrom.ToString("s")}&dateTimeTo={dateTimeTo.ToString("s")}");
            httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

            try
            {
                var response = await httpClient.SendAsync(httpRequestMessage);
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }
                string result = await response.Content.ReadAsStringAsync();
                JToken jt = JToken.Parse(result);
                return jt.ToObject<ObservableCollection<ApplicationView>>();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<HttpStatusCode> PostNewItem(ApplicationView application)
        {
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, "https://localhost:44376/api/Application");
            httpRequestMessage.Content = JsonContent.Create(application, typeof(ApplicationView));

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
        public async Task<HttpStatusCode> EditRecord(int id, string status, string jwt)
        {
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Put,
                                $"https://localhost:44376/api/Application/PutStatusApplication/{id}/{status}");
            //httpRequestMessage.Content = JsonContent.Create(new { status = status });
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
