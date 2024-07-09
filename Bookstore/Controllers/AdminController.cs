using Bookstore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Bookstore.Controllers
{
    public class AdminController : Controller
    {
        // Importing UsersDBContext as UsersDB
        readonly public DBContext UsersDB;

        // Initializes UsersDB to an Instance
        public AdminController(DBContext UsersDBObject)
        {
            UsersDB = UsersDBObject;
        }

        public IActionResult Dashboard()
        {
            if (!IsLogin())
            {
                return RedirectToAction("SignIn", "Account");
            }
            else if (!IsAdmin()){
                return RedirectToAction("NotAdmin");
            }

            return View();
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

            ViewModel UsersList = new ViewModel();
            UsersList.RegisteredUsers = UsersDB.Users.ToList();



            return View();
        }


        public IActionResult Books()
        {
            return View();
        }

        public IActionResult Team()
        {
            return View();
        }

        public String NotAdmin()
        {
            return "You Dont Have Access To This Page";
        }


        private bool IsAdmin()
        {
            string ThisUser_Username = HttpContext.Session.GetString("Usr").ToString();
            var UserDetails = UsersDB.Users.Where(x => x.Username == ThisUser_Username).FirstOrDefault();
            RoleType Admin = RoleType.Admin;

            if (UserDetails.role == Admin)
            {
                return true;
            }
            else
            {
                ViewData["IsAdmin"] = "False";
                return false;
            }
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
    }   
}
