using Bookstore.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;


namespace Bookstore.Controllers
{
    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Catalog()
        {
            if (HttpContext.Session.GetString("Usr") == null)
            {
                return RedirectToAction("SignIn", "Account");
            }
            else
            {
                return View();
            }
            
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
