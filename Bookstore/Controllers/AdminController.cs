﻿using Bookstore.Models;
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
            var ActivityList = db.AdminActivity.ToList();

            // Creating DropDown List
            UsersViewModel model = new UsersViewModel()
            {
                ListOfRoles = new List<SelectListItem>
                {
                    new SelectListItem {Value = "0", Text = "User"},
                    new SelectListItem {Value = "1", Text = "Admin"}
                }

            };

            // Returning User Delete Response IF CALL IS SENT FROM DASHBOARD
            if ((bool?)TempData["Exist"] == false)
            {
                model.Exist = false;
            }
            else if ((bool?)TempData["Exist"] == true)
            {
                model.Exist = true;
            }


            ViewBag.dropdown = model;
            return View(ActivityList);
        }

        public IActionResult BooksManagement()
        {
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
                    ActivityMessage = "Created A New " + NewEntry.role.ToString() + " '" + NewEntry.Username.ToString() + "'",
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
                    ActivityMessage = "Deleted The " + UsrToRemove.role.ToString() + " '" + UsrToRemove.Username.ToString() + "'",
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

    
       

        public IActionResult Team()
        {
            return View();
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
            return false;
            
        }

    }   
}
