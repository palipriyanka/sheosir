using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace NewCore3xMVC.Controllers
{
    public class ErrorHandlingController : Controller
    {
        public IActionResult Index()
        {
            try
            {
                var z = 0;
                return Content((1 / z).ToString());
            }
            catch (Exception ce)
            {
                throw;
            }
            return View();
        }
    }
}