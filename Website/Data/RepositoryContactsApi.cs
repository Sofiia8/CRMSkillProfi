using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using System;
using Website.Models;

namespace Website.Data
{
    public class RepositoryContactsApi : IRepositoryContacts
    {
        public static HttpClient httpClient;
        public RepositoryContactsApi()
        {
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<Dictionary<string, string[]>> GetItems()
        {
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get,
                        $"https://localhost:44376/api/Contacts");

            try
            {
                var response = await httpClient.SendAsync(httpRequestMessage);
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }
                string result = await response.Content.ReadAsStringAsync();
                JToken jt = JToken.Parse(result);
                var contacts = jt.ToObject<IEnumerable<Contacts>>();
                Dictionary<string, string[]> dict = new Dictionary<string, string[]>();
                foreach (var contact in contacts)
                {
                    dict.Add(contact.descriptionField, new string[2] { contact.codeField.ToString(), contact.valueField });
                }
                return dict;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<Contacts> GetContactByCode(int code, string jwt)
        {
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get,
                        $"https://localhost:44376/api/Contacts/GetContactByCode/{code}");
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
                return jo.ToObject<Contacts>();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<HttpStatusCode> EditContactById(int id, string newValue, string jwt)
        {
            string escaped = Uri.EscapeDataString(newValue);
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Put,
                        $"https://localhost:44376/api/Contacts/PutContactById/{id}/{escaped}");

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
