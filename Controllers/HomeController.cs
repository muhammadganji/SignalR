using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.Security.Claims;
using WebAppSignalR.Models;
using WebAppSignalR.Models.Entities;
using WebAppSignalR.Models.Services;

namespace WebAppSignalR.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserService _userService;

        public HomeController(ILogger<HomeController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;

        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Support");
            }

            return View();
        }

        [HttpPost]
        public IActionResult Login(User user)
        {


            var findUser = _userService.Exist(user);
            if (findUser != null)
            {

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, findUser.Username),
                    new Claim(ClaimTypes.NameIdentifier, findUser.Id.ToString()),
                };
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var properties = new AuthenticationProperties
                {
                    RedirectUri = Url.Content("/support")
                };
                return SignIn(new ClaimsPrincipal(identity),
                    properties, CookieAuthenticationDefaults.AuthenticationScheme);
            }
            else { return View(user); }
        }







        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}