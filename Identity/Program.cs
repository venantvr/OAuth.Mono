using System;
using Microsoft.Owin;
using Owin;
using Microsoft.Owin.Hosting;
using System.Web.Http;
using System.Collections.Generic;
using OAuth20;
using System.Net.Http;
using System.Text;
using System.Net.Http.Headers;
using System.Net.Http.Formatting;

namespace WebService
{
    class Program
    {
        private static string _targetUrl = "http://127.0.0.1:1234/api/message";
        private static string _tokenUrl = "http://127.0.0.1:1234/token";

        static void Main(string[] args)
        {
            string baseAddress = "http://*:1234";  

            // Start OWIN host     
            using (WebApp.Start<Startup>(url: baseAddress))
            {  
                var client = new HttpClient();  
                var response = client.GetAsync(_targetUrl).Result;  
                Console.WriteLine(response);  

                Console.WriteLine();  

                var authorizationHeader = Convert.ToBase64String(Encoding.UTF8.GetBytes(Startup.UserName + ":" + Startup.Password));  
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authorizationHeader);  

                var form = new Dictionary<string, string>
                {  
                    { "grant_type", "password" },  
                    { "username", Startup.UserName },  
                    { "password", Startup.Password },  
                };  

                var tokenResponse = client.PostAsync(_tokenUrl, new FormUrlEncodedContent(form)).Result;  
                var token = tokenResponse.Content.ReadAsAsync<Token>(new[] { new JsonMediaTypeFormatter() }).Result;  

                Console.WriteLine("Token issued is: {0}", token.AccessToken);  
                Console.WriteLine();  

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);  
                var authorizedResponse = client.GetAsync(_targetUrl).Result;  
                Console.WriteLine(authorizedResponse);  
                Console.WriteLine(authorizedResponse.Content.ReadAsStringAsync().Result);    

                Console.ReadKey();
            }  
                
            //string baseUrl = "http://*:1234";

            //using (WebApp.Start<Startup>(baseUrl))
            //{
            //    Console.WriteLine("Press Enter to quit.");
            //    Console.ReadKey();
            //}
        }
    }
}
