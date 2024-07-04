using Bookstore.Models;
using Microsoft.AspNetCore.Mvc;

namespace Bookstore.Controllers
{
    public class AccountController : Controller
    {
        // Importing UsersDBContext as UsersDB
        readonly public UsersDbContexxt UsersDB;

        // Initializes UsersDB to an Instance
        public AccountController(UsersDbContexxt UsersDBOject)
        {
            UsersDB = UsersDBOject;     
        }

        public IActionResult SignUp()
        {
            return View();
        }

        //SignUp Form Returning Data
        [HttpPost]
        public IActionResult SignUp(UsersModel NewUser)
        {

            bool IsUsernameExists = UsersDB.Users.Any(UserObj => UserObj.Username == NewUser.Username);

            // Makes sure form was valid and primary key wasnt repeated
            if (ModelState.IsValid && !IsUsernameExists)
            {
                UsersDB.Users.Add(NewUser);

                UsersDB.SaveChanges();
                ModelState.Clear();
                // Success Message
                TempData["SignUp_Success"] = "Your Account Has Been Created";
                return RedirectToAction("Index", "SignIn");
            }
            else if (IsUsernameExists)
            {
                TempData["UsernameUsed"] = "The Username Is Already Taken";
            }
            return View();
        }



        // Sign In
        public IActionResult SignIn()
        {
            TempData["IsLoginFail"] = null; // Login Hasnt Failed On First Get Request
            return View();
        }

        [HttpPost]
        public IActionResult SignIn(Cls_SignInFields IsUser)
        {

            if (ModelState.IsValid)
            {
                var TempUser = UsersDB.Users.Where(DB_Item => DB_Item.Username == IsUser.Username && DB_Item.Password == IsUser.Password).FirstOrDefault();
                if (TempUser != null)
                {
                    HttpContext.Session.SetString("UserSession", TempUser.Username);
                    TempData["IsLoginFail"] = null;
                    return RedirectToAction("logout");
                }
                else
                {
                    TempData["IsLoginFail"] = "Login Failed";
                    return View();
                }
            }
            return View();
        }
        
        
        public IActionResult MyAccount()
        {
            return View();
        }
    }
}
