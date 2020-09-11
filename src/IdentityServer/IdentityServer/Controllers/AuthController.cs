using IdentityServer.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace IdentityServer.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AuthController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel login)
        {
            // check if model is valid

            var result = await _signInManager.PasswordSignInAsync(login.UserName, login.Password, false, false);

            if (result.Succeeded)
            {
                return Redirect(login.ReturnUrl);
            }
            else if (result.IsLockedOut)
            {

            }

            return View();
        }

        [HttpGet]
        public IActionResult Register(string returnUrl)
        {
            return View(new RegisterViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel register)
        {
            // check if model is valid
            if (!ModelState.IsValid)
                return View(register);

            var user = new IdentityUser(register.UserName);

            var result = await _userManager.CreateAsync(user, register.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);

                return Redirect(register.ReturnUrl);
            }

            return View();
        }
    }
}
