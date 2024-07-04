using Microsoft.AspNetCore.Mvc;

namespace Bookstore.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
