using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyTravel.Models;
using MyTravel.ViewModels;

namespace MyTravel.Controllers
{
    public class AuthController : Controller
    {
        private SignInManager<TravelUser> _signInManager;

        public AuthController (SignInManager<TravelUser> signInManager)
        {
            _signInManager = signInManager;
        }

        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Trips", "App");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var signInResult = await _signInManager.PasswordSignInAsync(model.Username, model.Password, true, false);
                if (signInResult.Succeeded)
                {
                    if (string.IsNullOrEmpty(returnUrl))
                        return RedirectToAction("Trips", "App");
                    else
                        return Redirect(returnUrl);
                }
                else 
                {
                    ModelState.AddModelError("", "使用者帳號或密碼錯誤，請重新登入");
                }
            }

            return View();
        }

        public async Task<IActionResult> Logout()
        {
            if (User.Identity.IsAuthenticated)
            {
                await _signInManager.SignOutAsync();
            }
            return RedirectToAction("Index", "App");
        }
    }
}