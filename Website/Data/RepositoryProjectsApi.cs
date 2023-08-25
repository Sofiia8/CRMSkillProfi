using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using Website.Models;
using System.Net;
using System.Net.Http.Json;

namespace Website.Data
{
    public class RepositoryProjectsApi: IRepositoryProjects
    {
        public static HttpClient httpClient;
        public RepositoryProjectsApi()
        {
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }


        public async Task<IEnumerable<Project>> GetItems()
        {
            HttpRequestMessage httpRequestMessage =
                    new HttpRequestMessage(HttpMethod.Get, "https://localhost:44376/api/Projects");

            try
            {
                var response = await httpClient.SendAsync(httpRequestMessage);
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }
                string result = await response.Content.ReadAsStringAsync();
                JToken jt = JToken.Parse(result);
                return jt.ToObject<List<Project>>();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<Project> GetItemById(int id)
        {
            HttpRequestMessage httpRequestMessage =
                    new HttpRequestMessage(HttpMethod.Get, $"https://localhost:44376/api/Projects/{id}");

            try
            {
                var response = await httpClient.SendAsync(httpRequestMessage);
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }
                string result = await response.Content.ReadAsStringAsync();
                JObject jo = JObject.Parse(result);
                return jo.ToObject<Project>();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<HttpStatusCode> AddNewProject(Project project, string jwt)
        {
            HttpRequestMessage httpRequestMessage =
                new HttpRequestMessage(HttpMethod.Post, $"https://localhost:44376/api/Projects");

            httpRequestMessage.Content = JsonContent.Create(project, typeof(Project));
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

        public async Task<HttpStatusCode> EditProject (int id, Project project, string jwt)
        {
            HttpRequestMessage httpRequestMessage =
                new HttpRequestMessage(HttpMethod.Put, $"https://localhost:44376/api/Projects/{id}");

            httpRequestMessage.Content = JsonContent.Create(project, typeof(Project));
            httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

            try
            {
                var response = await httpClient.SendAsync(httpRequestMessage);
                return response.StatusCode;
            }
            catch(Exception ex)
            {
                return HttpStatusCode.ServiceUnavailable;
            }
        }

        public async Task<HttpStatusCode> DeleteProject(int id, string jwt)
        {
            HttpRequestMessage httpRequestMessage =
                new HttpRequestMessage(HttpMethod.Delete, $"https://localhost:44376/api/Projects/{id}");

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
