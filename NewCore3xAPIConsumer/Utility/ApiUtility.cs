using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace NewCore3xAPIConsumer.Utility
{
    public class ApiUtility
    {
        private readonly IConfiguration _configuration;
        private static string _apiDomain;

        public ApiUtility(IConfiguration configuration)
        {
            _configuration = configuration;
            _apiDomain = _configuration.GetValue<string>("AppSettings:ApiPath");
        }

        public async Task<string> Login(string userName, string password)
        {
            string api = _apiDomain + "api/UserDetails/Login?userName=" + userName + "&password=" + password;
            string apiResponse = string.Empty;
            using (var client = new HttpClient())
            {
                using (var response = await client.GetAsync(api))
                {
                    apiResponse = await response.Content.ReadAsStringAsync();
                }
            }
            return apiResponse;
        }


        public async Task<string> GetAPIResponseAsync(string bearerTokenString, string apiUrlToCall)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", bearerTokenString);
                var fullApiPath = _apiDomain + apiUrlToCall;
                using (var response = await client.GetAsync(fullApiPath))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    return apiResponse;
                }
            }
        }


        public async Task<string> PostAPIResponseAsync(string bearerTokenString, string apiUrlToCall, StringContent stringContent)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", bearerTokenString);
                var fullApiPath = _apiDomain + apiUrlToCall;
                using (var response = await client.PostAsync(fullApiPath, stringContent))
                {
                    var res = await response.Content.ReadAsStringAsync();
                    return res;
                }
            }
        }

    }
}
