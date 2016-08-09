using Microsoft.AspNetCore.Mvc;

namespace MyTravel.Controllers.Web 
{
    public class AppController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
} 