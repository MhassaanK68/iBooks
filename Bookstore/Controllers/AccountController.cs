using Bookstore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace Bookstore.Controllers
{
    public class AccountController : Controller
    {
        // Importing dbContext as db
        readonly public DBContext db;

        // Initializes db to an Instance
        public AccountController(DBContext dbOject)
        {
            db = dbOject;     
        }

        public IActionResult SignUp()
        {
            return View();
        }

        //SignUp Form Returning Data

        [HttpPost]
        public IActionResult SignUp(Cls_SignUpFields NewUser)
        {

            bool IsUsernameExists = db.Users.Any(UserObj => UserObj.Username == NewUser.Username);

            // Makes sure form was valid and primary key wasnt repeated
            if (ModelState.IsValid && !IsUsernameExists)
            {
                // Converting SignUp Model To User Model
                UsersModel UsrToAdd = new UsersModel();
                UsrToAdd.Username = NewUser.Username;
                UsrToAdd.Password = NewUser.Password;
                UsrToAdd.Email = NewUser.Email;
                UsrToAdd.role = RoleType.User;

                // Adding User Model Instance
                db.Users.Add(UsrToAdd);
                db.SaveChanges();
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
                var TempUser = db.Users.Where(DB_Item => DB_Item.Username == IsUser.Username && DB_Item.Password == IsUser.Password).FirstOrDefault();
                if (TempUser != null)
                {
                    HttpContext.Session.SetString("Usr", TempUser.Username);

                    TempData["IsLoginFail"] = null;
                    // If user is an admin redirect to admin panel
                    if (TempUser.role == RoleType.Admin)
                    {
                        AdminActivityModel NewActivity = new() 
                        { 
                        Activity = AdminActivityType.PanelLogin,
                        ActivityMessage = IsUser.Username + " Logged In",
                        MetaData = "Signed In",
                        Time = DateTime.Now,
                        AdminName = IsUser.Username
                        };

                        db.AdminActivity.Add(NewActivity);
                        db.SaveChanges();
                        return RedirectToAction("Dashboard", "Admin");
                    }

                    //Otherwise redirect to home
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

        public IActionResult Logout()
        {
            if (HttpContext.Session.GetString("Usr") != null)
            {
                AdminActivityModel NewActivity = new() { Activity = AdminActivityType.PanelLogout, AdminName = HttpContext.Session.GetString("Usr"), Time = DateTime.Now, MetaData = "Sign Out", ActivityMessage = HttpContext.Session.GetString("Usr") + " Signed Out" };
                db.AdminActivity.Add(NewActivity);
                db.SaveChanges();
                HttpContext.Session.Remove("Usr");
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("SignIn");
            }
        }

    }
}
