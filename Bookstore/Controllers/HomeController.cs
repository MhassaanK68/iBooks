using Bookstore.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;


namespace Bookstore.Controllers
{
    public class HomeController : Controller
    {
        public readonly DBContext db;
        public HomeController(DBContext dbObj)
        {
            db = dbObj;
        }


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
                List<BooksViewModel> ViewBooks = new List<BooksViewModel>()
                {
                    new BooksViewModel { CategoryName = "Sci_Fi",    BookCategory = db.Books.Where(book => book.Category == "Sci_Fi")    },
                    new BooksViewModel { CategoryName = "Adventure", BookCategory = db.Books.Where(book => book.Category == "Adventure") },
                    new BooksViewModel { CategoryName = "Romantic",  BookCategory = db.Books.Where(book => book.Category == "Romantic")  },
                    new BooksViewModel { CategoryName = "Horror",    BookCategory = db.Books.Where(book => book.Category == "Horror")    },
                    new BooksViewModel { CategoryName = "Humor",     BookCategory = db.Books.Where(book => book.Category == "Humor")     },
                    new BooksViewModel { CategoryName = "Fantasy",   BookCategory = db.Books.Where(book => book.Category == "Fantasy")   },
                    new BooksViewModel { CategoryName = "Thrillers", BookCategory = db.Books.Where(book => book.Category == "Thrillers") }
                };

                foreach (var item in ViewBooks)
                {
                    if (item.BookCategory.Count() == 0)
                    {
                        item.CategoryName = null;
                    }
                }
                return View(ViewBooks);
            };            
            
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
