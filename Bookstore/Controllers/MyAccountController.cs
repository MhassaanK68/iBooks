using Microsoft.AspNetCore.Mvc;

namespace Bookstore.Controllers
{
    public class MyAccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
