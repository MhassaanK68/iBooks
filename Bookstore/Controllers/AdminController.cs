using Bookstore.Migrations;
using Bookstore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NuGet.Protocol;
using System.Linq;

namespace Bookstore.Controllers
{
    public class AdminController : Controller
    {



        // Importing dbContext as db
        readonly public DBContext db;
        readonly public IWebHostEnvironment _webHostEnvironment;

        // Initializes db to an Instance
        public AdminController(DBContext dbObject, IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
            db = dbObject;
        }


        // DASHBOARD ACTION METHOD

        [Route("/admin")]
        [Route("/admin/dashboard")]
        public IActionResult Dashboard()
        {
            if (!IsLogin())
            {
                return RedirectToAction("SignIn", "Account");
            }
            else if (!IsAdmin()){
                return RedirectToAction("NotAdmin");
            }

            // Importing All Activities From Database
            var ActivityList = db.AdminActivity.OrderByDescending(x => x.Time).Take(5);

            // Creating DropDown List
            UsersViewModel model = new UsersViewModel()
            {
                ListOfRoles = new List<SelectListItem>
                {
                    new SelectListItem {Value = "0", Text = "User"},
                    new SelectListItem {Value = "1", Text = "Admin"}
                }

            };
            ViewBag.dropdown = model;

            // Returning User Delete Response IF CALL IS SENT FROM DASHBOARD
            
            if ((bool?)TempData["Exist"] == false)
            {
                model.Exist = false;
            }
            else if ((bool?)TempData["Exist"] == true)
            {
                model.Exist = true;
            }



            List<String> CategoryList = new List<String>() { "Sci-Fi", "Adventure", "Romantic", "Horror", "Humor", "Fantasy", "Thrillers" };
            ViewBag.CategoryDropdown = CategoryList;
            ViewBag.BooksInputModel = new BooksInputModel();

            return View(ActivityList);
        }



        public IActionResult BooksManagement()
        {

            List<String> CategoryList = new List<String>() { "Sci-Fi", "Adventure", "Romantic", "Horror", "Humor", "Fantasy", "Thrillers"};
            ViewBag.CategoryDropdown = CategoryList;

            return View();
        }

        [HttpPost]
        public IActionResult CreateBook(BooksInputModel NewBook)
        {
            if (ModelState.IsValid)
            {
                if (NewBook.CoverPhoto != null)
                {
                    string RootPath = _webHostEnvironment.WebRootPath;
                    string ImageName = "books/cover/" + Guid.NewGuid().ToString() + NewBook.CoverPhoto.FileName;
                    string ImagePath = Path.Combine(RootPath, ImageName);
                    NewBook.CoverPhoto.CopyTo(new FileStream(ImagePath, FileMode.Create));


                    //Creating Book To Add
                    BooksModel BookToAdd = new BooksModel()
                    { BookName = NewBook.BookName, Author = NewBook.Author, Category = NewBook.Category, ImageSource = "/" + ImageName};

                    //Creating Activity
                    AdminActivityModel NewActivity = new AdminActivityModel()
                    { Activity = AdminActivityType.CreateBook,
                        AdminName = HttpContext.Session.GetString("Usr"),
                        MetaData = NewBook.BookName,
                        ActivityMessage = "New Book Added",
                        Time = DateTime.Now
                    };

                    db.Books.Add(BookToAdd);
                    db.AdminActivity.Add(NewActivity);
                    db.SaveChanges();
                    ModelState.Clear();

               }

            }

            return RedirectToAction("BooksManagement");
        }

        [HttpPost]
        public IActionResult DeleteBook(BooksInputModel BookNameToDelete)
        {
            if (db.Books.Any(ThisBook => ThisBook.BookName == BookNameToDelete.BookName))
            {
                BooksModel BookToDelete = db.Books.First(ThisBook => ThisBook.BookName == BookNameToDelete.BookName);
                string ImageRootPath = _webHostEnvironment.WebRootPath + BookToDelete.ImageSource;

                AdminActivityModel NewActivity = new()
                {
                    AdminName = HttpContext.Session.GetString("Usr"), Activity = AdminActivityType.DeleteBook, ActivityMessage = "Book Removed", MetaData = BookNameToDelete.BookName, Time = DateTime.Now
                };

                db.Add(NewActivity);
                db.Remove(BookToDelete);
                //System.IO.File.Delete(ImageRootPath);
                db.SaveChanges();
                TempData["BookDelete"] = "true";
            }
            else
            {
                TempData["BookDelete"] = "false";
            }
            return RedirectToAction("BooksManagement");
        }

        [HttpPost]
        public IActionResult ScheduleBook()
        {
            return View();
        }

        public string GetBooks()
        {
            var booknames = db.Books.ToList();
            var json = JsonConvert.SerializeObject(booknames);
            return json;
        }




        public IActionResult Users()
        {
            if (!IsLogin())
            {
                return RedirectToAction("SignIn", "Account");
            }
            else if (!IsAdmin())
            {
                return RedirectToAction("NotAdmin");
            }

            UsersViewModel model = new UsersViewModel()
            {
                ListOfRoles = new List<SelectListItem>
                {
                    new SelectListItem {Value = "0", Text = "User"},
                    new SelectListItem {Value = "1", Text = "Admin"}
                }
                  
            };

            // IF CALL IS SENT FROM ADMINISTRATION
            if ((bool?)TempData["Exist"] == false)
            {
                model.Exist = false;
            }
            else if ((bool?)TempData["Exist"] == true)
            {
                model.Exist = true;
            }

            




            return View(model);
        }

        [HttpPost]
        public IActionResult CreateUser(UsersViewModel NewEntry)
        {
            if (ModelState.IsValid)
            {

                UsersModel TempUser = new UsersModel()
                {
                    Username = NewEntry.Username,
                    Email = NewEntry.Email,
                    Password = NewEntry.Password,
                    role = NewEntry.role

                };

                AdminActivityModel NewActivity = new AdminActivityModel()
                {
                    AdminName = HttpContext.Session.GetString("Usr"),
                    Activity = AdminActivityType.CreateUser,
                    MetaData = NewEntry.Username,
                    ActivityMessage = "New " + NewEntry.role.ToString() + " Created",
                    Time = DateTime.Now

                };

                db.Users.Add(TempUser);
                db.AdminActivity.Add(NewActivity);
                db.SaveChanges();
                ModelState.Clear();

                return RedirectToAction("Dashboard");
            }




            return View(NewEntry);
        }

        [HttpPost]
        public IActionResult DeleteForm(UsersViewModel NewEntry)
        {
            if (db.Users.Any(x => x.Username == NewEntry.Username))
            {
                UsersModel UsrToRemove = db.Users.First(x => x.Username == NewEntry.Username);

                AdminActivityModel NewActivity = new AdminActivityModel()
                {
                    AdminName = HttpContext.Session.GetString("Usr"),
                    Activity = AdminActivityType.DeleteUser,
                    MetaData = UsrToRemove.Username,
                    ActivityMessage = UsrToRemove.role.ToString() + " Removed",
                    Time = DateTime.Now
                };

                db.Users.Remove(UsrToRemove);
                db.AdminActivity.Add(NewActivity) ;
                db.SaveChanges();
                if (NewEntry.Username != NewActivity.AdminName)
                {
                    TempData["Exist"] = true;
                }
            }
            else 
            {
                TempData["Exist"] = false;
            }

            return RedirectToAction("Users");
        }

    
       



        public String NotAdmin()
        {
            return "You Dont Have Access To This Page";
        }

        private bool IsLogin()
        {
            if (HttpContext.Session.GetString("Usr") != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool IsAdmin()
        {
            if (IsLogin())
            {
                string ThisUser_Username = HttpContext.Session.GetString("Usr").ToString();
                var UserDetails = db.Users.Where(x => x.Username == ThisUser_Username).FirstOrDefault();
                RoleType Admin = RoleType.Admin;
                if (UserDetails != null && UserDetails.role == Admin) // ERROR HANDLING TO BE DONE
                {
                    return true;
                }
            }

            // If NotLogIn + NotAdmin

            ViewData["IsAdmin"] = "False";
            NotAdmin();
            return false;
            
        }


        public string GetBookNames()
        {
            var booknames = db.Books.Select(x => x.BookName);
            var json = JsonConvert.SerializeObject(booknames);
            return json;
        }

    }   
}
