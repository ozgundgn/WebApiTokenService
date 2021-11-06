using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Models;

namespace DataAccess
{
    public class Client
    {
        private static HttpClient HttpClient;
        public static string SetToken { set; get; }
        
        public static HttpClient GetClient()
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            HttpClient = new HttpClient(clientHandler) { BaseAddress = new Uri("https://localhost:44307/") };
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*")); //hepisini kabul etmesi için

            if (SetToken != null)
            {
                HttpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + SetToken);
            }

            return HttpClient;
        }
    }
}
