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
        //https://stackoverflow.com/questions/60520664/how-to-create-a-simple-login-in-asp-net-core-without-database-and-authorize-cont

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

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction(nameof(Login));
        }
    }
}
