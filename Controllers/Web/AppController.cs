using System;
using Microsoft.AspNetCore.Mvc;

namespace MyTravel.Controllers.Web 
{
    public class AppController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Contact()
        {
            throw new InvalidOperationException("Something went wrong!");
            return View();
        }

        public IActionResult About()
        {
            return View();
        }
    }
} 