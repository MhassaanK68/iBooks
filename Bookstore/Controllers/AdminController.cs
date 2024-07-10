using Bookstore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Bookstore.Controllers
{
    public class AdminController : Controller
    {

        bool? Exist;

        // Importing dbContext as db
        readonly public DBContext db;

        // Initializes db to an Instance
        public AdminController(DBContext dbObject)
        {
            db = dbObject;
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

            UsersViewModel model = new UsersViewModel()
            {
                ListOfRoles = new List<SelectListItem>
                {
                    new SelectListItem {Value = "0", Text = "User"},
                    new SelectListItem {Value = "1", Text = "Admin"}
                }
                  
            };

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
        public IActionResult Users(UsersViewModel NewEntry)
        {
            //bool showModal = false;

            //// Check session to determine if modal should be shown
            //if (HttpContext.Session.GetString("ShowModal") == "true")
            //{
            //    showModal = true;
            //}


            if (ModelState.IsValid)
            {
                if (db.Users.Any(Entry => Entry.Username == NewEntry.Username))
                {
                    var ThisUser = db.Users.First(Users => Users.Username == NewEntry.Username);
                    ThisUser.Password = NewEntry.Password;
                    ThisUser.Email = NewEntry.Email;
                    ThisUser.role = NewEntry.role;
                    db.SaveChanges();
                }
                else {
                    UsersModel TempUser = new UsersModel()
                    {
                        Username = NewEntry.Username,
                        Email = NewEntry.Email,
                        Password = NewEntry.Password,
                        role = NewEntry.role
                    };

                    db.Users.Add(TempUser);
                    db.SaveChanges();
                    ModelState.Clear();


                    return RedirectToAction("Dashboard");
                }
            }




            return View(NewEntry);
        }

        [HttpPost]
        public IActionResult DeleteForm(UsersViewModel NewEntry)
        {
            if (db.Users.Any(x => x.Username == NewEntry.Username))
            {
                UsersModel UsrToRemove = db.Users.First(x => x.Username == NewEntry.Username);
                db.Users.Remove(UsrToRemove);
                db.SaveChanges();
                TempData["Exist"] = true;
            }
            else 
            {
                TempData["Exist"] = false;
            }

            return RedirectToAction("Users");
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
            var UserDetails = db.Users.Where(x => x.Username == ThisUser_Username).FirstOrDefault();
            RoleType Admin = RoleType.Admin;

            if (UserDetails.role == Admin) // ERROR HANDLING TO BE DONE
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
