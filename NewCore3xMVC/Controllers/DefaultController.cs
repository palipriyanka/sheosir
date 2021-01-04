using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace NewCore3xMVC.Controllers
{
    public class DefaultController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        
        public IActionResult PartialViewTest()
        {
            return View();
        }

        public IActionResult ComponentTest()
        {
            return View();
        }
    }
}