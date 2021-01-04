using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewCore3xMVC.DependencyInjection;
using System;

namespace NewCore3xMVC.Controllers
{
    public class DIController : Controller
    {
        private readonly IAgeCalculator _ageCalculator;

        public DIController(IAgeCalculator ageCalculator)
        {
            _ageCalculator = ageCalculator;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(IFormCollection form)
        {
            var dob = DateTime.Parse(form["YourAge"]);
            ViewBag.DOB = dob;
            ViewBag.YourAge = _ageCalculator.GetMyAge(dob);
            return View();
        }
    }
}