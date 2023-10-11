using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProductsManage.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsManage.Controllers
{
    [Authorize]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new GeneralModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public IActionResult Logout()
        {
            var login = HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Json(login);
        }

        [HttpPost]
        public async Task<IActionResult> ProductAdd(ProductInfo item)
        {
            ProductInfo info = new ProductInfo();
            info.id = -1;
            if (item != null && item.ProductName!=null)
            {
                item.userid = Convert.ToInt32(HttpContext.Session.GetString("userid"));
                item.TotalAmount = item.ProductPrice * item.ProductQuantity;
                using (var context = new ApplicationDbContext())
                {
                    context.product.Add(item);
                    await context.SaveChangesAsync();
                    info.TotalAmount = item.TotalAmount;
                    info.id = item.id;
                }
            }
           return Json(info);
        }
        [HttpPost]
        public async Task<IActionResult> BindGrid()
        {
            int userid = Convert.ToInt32(HttpContext.Session.GetString("userid"));
            List<ProductInfo> product = new List<ProductInfo>();
            if (userid != 0)
            {
                using (var context = new ApplicationDbContext())
                {
                    product=context.product.Where(item=>item.userid==userid).ToList();
                }
            }
            return Json(product);
        }

        [HttpGet]
        public async Task<IActionResult> GetOnclickUserData(string uniqueproductid)
        {
            int userid = Convert.ToInt32(HttpContext.Session.GetString("userid"));
            ProductInfo product = new ProductInfo();
            if (userid != 0)
            {
                using (var context = new ApplicationDbContext())
                {
                    product = context.product.Where(item => item.userid == userid && item.id==Convert.ToInt32(uniqueproductid)).FirstOrDefault();
                }
            }
            return Json(product);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteProduct(string uniqueproductid)
        {
            int userid = Convert.ToInt32(HttpContext.Session.GetString("userid"));
            ProductInfo product = new ProductInfo();
            if (userid != 0)
            {
                using (var context = new ApplicationDbContext())
                {
                    product = context.product.Find(Convert.ToInt32(uniqueproductid));
                    context.Remove(product);
                    context.SaveChangesAsync();
                }
            }
            return Json(product);
        }
    }
}
