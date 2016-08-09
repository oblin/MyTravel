using Microsoft.AspNetCore.Mvc;
using MyTravel.Services;
using MyTravel.ViewModels;

namespace MyTravel.Controllers.Web
{
    public class AppController : Controller
    {
        private IMailService _mailService;

        public AppController(IMailService mailService)
        {
            _mailService = mailService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Contact(ContactViewModel model)
        {
            _mailService.SendMail("servcie@example.com", model.Email, "From Traveling", model.Message);
            return View(model);
        }

        public IActionResult About()
        {
            return View();
        }
    }
} 