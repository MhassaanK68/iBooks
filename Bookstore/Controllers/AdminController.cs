using Bookstore.Models;
using Microsoft.AspNetCore.Mvc;

namespace Bookstore.Controllers
{
    public class AdminController : Controller
    {
        // Importing UsersDBContext as UsersDB
        readonly public UsersDbContexxt UsersDB;

        // Initializes UsersDB to an Instance
        public AdminController(UsersDbContexxt UsersDBObject)
        {
            UsersDB = UsersDBObject;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("UserSession") != null)
            {
                string ThisUser_Username = HttpContext.Session.GetString("UserSession").ToString();
                var UserDetails = UsersDB.Users.Where(x => x.Username == ThisUser_Username).FirstOrDefault();
                if (UserDetails.role == "Admin")
                {
                    return View();
                }
                else
                {
                    ViewData["IsAdmin"] = "False";
                    return View();
                }
            }
            else
            {
                return RedirectToAction("SignIn", "Account");
            }
        }


    }   
}
