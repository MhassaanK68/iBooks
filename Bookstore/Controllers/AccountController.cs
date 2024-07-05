using Bookstore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

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
        public IActionResult SignUp(Cls_SignUpFields NewUser)
        {

            bool IsUsernameExists = UsersDB.Users.Any(UserObj => UserObj.Username == NewUser.Username);

            // Makes sure form was valid and primary key wasnt repeated
            if (ModelState.IsValid && !IsUsernameExists)
            {
                // Converting SignUp Model To User Model
                UsersModel UsrToAdd = new UsersModel();
                UsrToAdd.Username = NewUser.Username;
                UsrToAdd.Password = NewUser.Password;
                UsrToAdd.Email = NewUser.Email;
                UsrToAdd.role = "User";

                // Adding User Model Instance
                UsersDB.Users.Add(UsrToAdd);
                UsersDB.SaveChanges();
                ModelState.Clear();

                // Success Message
                TempData["SignUp_Success"] = "Your Account Has Been Created";
                return RedirectToAction("SignIn", "Account");
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
                    ViewData["Session"] = HttpContext.Session.Id.ToString();
                    TempData["IsLoginFail"] = null;
                    // If user is an admin redirect to admin panel
                    if (TempUser.role == "Admin")
                    {
                        return RedirectToAction("Index", "Admin");
                    }
                    // Otherwise redirect to home
                    return RedirectToAction("Catalog", "Home");
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
            if (HttpContext.Session.GetString("UserSession") != null)
            {               
                return View();
            }
            else
            {
                return RedirectToAction("SignIn");
            }
        }
    }
}
