using Bookstore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

            var count = UsersDB.Users.Count(me => me.role == "User");
            ViewData["RegisteredUsers"] = count;

            List<UsersModel> AllUsers = UsersDB.Users.ToList();


            return View(AllUsers);
        }

        public String NotAdmin()
        {
            return "You Dont Have Access To This Page";
        }


        private bool IsAdmin()
        {
            string ThisUser_Username = HttpContext.Session.GetString("Usr").ToString();
            var UserDetails = UsersDB.Users.Where(x => x.Username == ThisUser_Username).FirstOrDefault();
            if (UserDetails.role == "Admin")
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
