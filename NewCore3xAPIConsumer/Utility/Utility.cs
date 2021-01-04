using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NewCore3xAPIConsumer.Utility
{
    public class Utility
    {
        private readonly IConfiguration _configuration;
        private static string _apiDomain;

        public Utility(IConfiguration configuration)
        {
            _configuration = configuration;
            _apiDomain = _configuration.GetValue<string>("AppSettings:ApiPath");
        }

        public string GenerateJSONToken()
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SomeSecurityTokenKey"));
            var credential = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var tokenString = new JwtSecurityToken(
                issuer: _apiDomain,
                audience: _apiDomain,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: credential
                );
            return new JwtSecurityTokenHandler().WriteToken(tokenString);
        }

        public static string GetBearerTokenString(HttpContext httpContext)
        {
            var token = httpContext.Session.GetString("BearerToken");
            if (string.IsNullOrEmpty(token))
            {
                return string.Empty;
            }
            else
            {
                return token;
            }
        }
    }
}
