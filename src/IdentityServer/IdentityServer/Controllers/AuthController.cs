using IdentityServer.ViewModels;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServer.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IIdentityServerInteractionService _interactionService;

        public AuthController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IIdentityServerInteractionService interactionService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _interactionService = interactionService;
        }

        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl)
        {
            var externalProviders = await _signInManager.GetExternalAuthenticationSchemesAsync();

            return View(new LoginViewModel
            {
                ReturnUrl = returnUrl,
                ExternalProviders = externalProviders
            });
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

        [HttpGet]
        public async Task<IActionResult> Logout(string logoutId)
        {
            await _signInManager.SignOutAsync();

            var logoutRequest = await _interactionService.GetLogoutContextAsync(logoutId);

            if (string.IsNullOrEmpty(logoutRequest.PostLogoutRedirectUri))
            {
                return RedirectToAction("index", "Home");
            }

            return Redirect(logoutRequest.PostLogoutRedirectUri);
        }

        public async Task<IActionResult> ExternalLogin(string returnUrl, string provider)
        {
            var redirectUri = Url.Action(nameof(ExternalLoginCallback), "auth", new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUri);
            return Challenge(properties, provider);
        }

        public async Task<IActionResult> ExternalLoginCallback(string returnUrl)
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();

            if (info == null)
                return RedirectToAction("login");

            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false);

            if (result.Succeeded)
            {
                return Redirect(returnUrl);
            }

            var userName = info.Principal.FindFirst(ClaimTypes.Name).Value;

            return View("ExternalRegister", new ExternalRegisterViewModel
            {
                UserName = userName,
                ReturnUrl = returnUrl
            });
        }

        public async Task<IActionResult> ExternalRegister(ExternalRegisterViewModel vm)
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();

            if (info == null)
                return RedirectToAction("login");

            var user = new IdentityUser(vm.UserName);

            var result = await _userManager.CreateAsync(user);

            if (!result.Succeeded)
            {
                return View(vm);
            }

            var loginResult = await _userManager.AddLoginAsync(user, info);

            if (!loginResult.Succeeded)
            {
                return View(vm);
            }

            await _signInManager.SignInAsync(user, false);

            return Redirect(vm.ReturnUrl);
        }
    }
}
