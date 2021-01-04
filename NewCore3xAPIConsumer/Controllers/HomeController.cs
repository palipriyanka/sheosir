using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NewCore3xAPIConsumer.Models;

namespace NewCore3xAPIConsumer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string userName, string password)
        {
            var loginSuccess = await new Utility.ApiUtility(_configuration).Login(userName, password);

            if (bool.Parse(loginSuccess))
            {
                var bearerTokenString = new Utility.Utility(_configuration).GenerateJSONToken();
                HttpContext.Session.SetString("BearerToken", bearerTokenString);
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Message = "Sorry, username and password are wrong. Please try again.";
            return View();            
        }

        public IActionResult GetJWTToken()
        {
            return Content(Utility.Utility.GetBearerTokenString(HttpContext));
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("BearerToken");
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
