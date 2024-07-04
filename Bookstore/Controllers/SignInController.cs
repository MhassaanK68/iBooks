using Microsoft.AspNetCore.Mvc;
using Bookstore.Models;
using System.Net.Mime;
using Microsoft.AspNetCore.Http;

namespace Bookstore.Controllers
{
    public class SignInController : Controller
    {
        public readonly UsersDbContexxt UsersDB;

        public SignInController(UsersDbContexxt UserObject)
        {
            UsersDB = UserObject;
        }
        public IActionResult Index()
        {
            TempData["IsLoginFail"] = null;
            return View();
        }

        [HttpPost]
        public IActionResult Index(Users IsUser)
        {
            ModelState.Remove("Password2");
            ModelState.Remove("Email");
            if (ModelState.IsValid)
            {
                var TempUser = UsersDB.Users.Where(x => x.UserName == IsUser.UserName && x.Password == IsUser.Password).FirstOrDefault();
                if (TempUser != null)
                {
                    HttpContext.Session.SetString("UserSession", TempUser.UserName);
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

        public IActionResult logout()
        {
            return View();
        }
    }
}
