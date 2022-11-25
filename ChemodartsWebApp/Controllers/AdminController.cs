using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace ChemodartsWebApp.Controllers
{
    public class AdminController : Controller
    {

        private readonly IOptions<List<Models.User>> _users;
        public AdminController(IOptions<List<Models.User>> users)
        {
            _users = users;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login([Bind("UserName,Password")] Models.User userToLogin)
        {
            var user = _users.Value.Find(c => c.UserName == userToLogin.UserName && c.Password == userToLogin.Password);

            if (Models.User.Admin.UserName == userToLogin.UserName && Models.User.Admin.Password == userToLogin.Password)
            {
                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,userToLogin.UserName),
                new Claim("FullName", userToLogin.UserName),
                new Claim(ClaimTypes.Role, "Administrator"),
            };

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {

                    RedirectUri = "/Tournament/Index",

                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);
            }

            ViewBag.Message = "Falsche Admin-Daten!";
            return View();
        }

        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public ActionResult Login(LoginViewModel model, string returnUrl)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(model);
        //    }

        //    // we use simple forms authentication with a list of user in the web.config file 
        //    if (FormsAuthentication.Authenticate(model.UserName, model.Password))
        //    {
        //        FormsAuthentication.RedirectFromLoginPage(model.UserName, false);
        //    }
        //    ModelState.AddModelError("", "Wrong username or password");
        //    return View(model);
        //}
    }
}
