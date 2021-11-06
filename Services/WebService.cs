using Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DataAccess;
using Models.Results;
using Newtonsoft.Json;

namespace Services
{
    public class WebService
    {
        static string _serviceUrl = "";
        private static HttpClient _httpClient;
        
        public static async Task<string> GetAll(string method)
        {
            _httpClient = Client.GetClient();
            _serviceUrl = $"/api/users/{method}";

            using HttpResponseMessage responseMessage = await _httpClient.GetAsync(_serviceUrl);
            if (responseMessage.IsSuccessStatusCode)
            {
                return await responseMessage.Content.ReadAsStringAsync();
            }

            return default(string);
        }

        public static async Task<string> GetToken(User user)
        {
            _httpClient = Client.GetClient();
            _serviceUrl = "/api/users/login";
            List<KeyValuePair<string, string>> informs = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("FirstName", user.FirstName),
                new KeyValuePair<string, string>("LastName", user.LastName) 
            };

            HttpContent content = new StringContent(JsonConvert.SerializeObject(user),Encoding.UTF8, "application/json");

            using HttpResponseMessage responseMessage = await _httpClient.PostAsync(_serviceUrl,content);
               return await responseMessage.Content.ReadAsStringAsync();

        }
    }
}
