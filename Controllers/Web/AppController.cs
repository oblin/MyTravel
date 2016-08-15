using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MyTravel.Models;
using MyTravel.Services;
using MyTravel.ViewModels;

namespace MyTravel.Controllers.Web
{
    public class AppController : Controller
    {
        private IMailService _mailService;
        private IConfigurationRoot _config;
        private ITravelRepository _repository;
        private ILogger<AppController> _logger;

        public AppController(IMailService mailService, IConfigurationRoot config,
            ITravelRepository repository, ILogger<AppController> logger)
        {
            _mailService = mailService;
            _config = config;
            _repository = repository;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Trips()
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
            if (model.Email.Contains("aol.com"))
            {
                ModelState.AddModelError("", "We don't support AOL addresses");
            }
            if (ModelState.IsValid)
            {
                _mailService.SendMail(_config["MailSettings:ToAddress"], model.Email, "From Traveling", model.Message);
                ModelState.Clear();
                ViewBag.UserMessage = "Message Sent";
            }
            return View();
        }

        public IActionResult About()
        {
            return View();
        }
    }
}