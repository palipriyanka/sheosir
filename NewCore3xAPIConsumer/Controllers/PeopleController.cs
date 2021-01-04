using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NewCore3xAPIConsumer.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace NewCore3xAPIConsumer.Controllers
{

    public class PeopleController : Controller
    {
        string _apiDomain = string.Empty;
        private readonly IConfiguration _configuration;
        public PeopleController(IConfiguration configuration)
        {
            _configuration = configuration;
            _apiDomain = _configuration.GetValue<string>("AppSettings:ApiPath");
        }

        public async Task<IActionResult> Index()
        {
            var bearerTokenString = Utility.Utility.GetBearerTokenString(HttpContext);
            if (string.IsNullOrEmpty(bearerTokenString))
            {
                return RedirectToAction("Login", "Home");
            }

            IList<Person> people = new List<Person>();
            var apiResponse = await new Utility.ApiUtility(_configuration).GetAPIResponseAsync(bearerTokenString, "api/people");
            people = JsonConvert.DeserializeObject<List<Person>>(apiResponse);
            return View(people);
        }

        public IActionResult JavaScriptCall()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AutoId,FirstName,LastName,Age,Active")] Person person)
        {
            if (ModelState.IsValid)
            {
                var bearerTokenString = Utility.Utility.GetBearerTokenString(HttpContext);
                if (string.IsNullOrEmpty(bearerTokenString))
                {
                    return RedirectToAction("Login", "Home");
                }

                var jsonObject = JsonConvert.SerializeObject(person);
                var stringContent = new StringContent(jsonObject, System.Text.Encoding.UTF8, "application/json");

                var apiResponse = await new Utility.ApiUtility(_configuration).PostAPIResponseAsync(bearerTokenString, "api/people", stringContent);
                var response = apiResponse;
                return RedirectToAction(nameof(Index));
            }
            return View(person);
        }
    }
}