using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProductsManage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace ProductsManage.Controllers
{
    public class LoginController : Controller
    {


        readonly IHttpContextAccessor httpContextAccessor;
        readonly IHostingEnvironment hostingEnvironment;
        public LoginController(IHttpContextAccessor _httpcontextaccessor, IHostingEnvironment _hostingenvironment)
        {

            this.httpContextAccessor = _httpcontextaccessor;
            this.hostingEnvironment = _hostingenvironment;
        }
        public IActionResult Index()
        {
            using (var context = new ApplicationDbContext())
            {
                var user = context.user.OrderByDescending(e => e.id).FirstOrDefault();
                if (user == null)
                {

                    var users = new List<Login>()
                {
                    new Login()
                    {
                    EmailID = "revana22@gmail.com",
                    Password = "123456789"
                    },
                    new Login()
                    {
                    EmailID = "chethan22@gmail.com",
                    Password = "334455"
                    }
                };
                    context.user.AddRange(users);
                    context.SaveChanges();
                }
            }
            if (TempData["ErrorMessage"] != null)
            {
                ViewBag.message = TempData["ErrorMessage"];
                TempData["ErrorMessage"] = null;
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> VerifingEmailandPassword(Login item)
        {
            using (var context = new ApplicationDbContext())
            {
                var userinfo = context.user.Where(e => e.EmailID.Contains(item.EmailID) && e.Password.Contains(item.Password)).FirstOrDefault();

                if (userinfo != null)
                {
                    var userclaims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.Actor,item.Password)
                    };
                    var grantidentity = new ClaimsIdentity(userclaims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var userprinciple = new ClaimsPrincipal(grantidentity);
                    var props = new AuthenticationProperties();
                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userprinciple, props).Wait();
                    Thread.CurrentPrincipal = userprinciple;
                    HttpContext.Session.SetString("userid", userinfo.id.ToString());
                    return RedirectToAction("Index", "Home");

                }
            }
            TempData["ErrorMessage"] = "Invalid credentials, please try with valid credentials";

            return RedirectToAction("Index", "Login");
        }

    }
}
