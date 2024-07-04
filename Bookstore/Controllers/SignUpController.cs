using Bookstore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Bookstore.Controllers
{
    public class SignUpController : Controller
    {
        // Importing UsersDB
        private readonly UsersDbContexxt _UsersDB;

        
        public SignUpController(UsersDbContexxt UsersDB)
        {
            // Intializing Users DB To An Instance Of Itself
            _UsersDB = UsersDB;
            
        }

        //SignUp Page
        public IActionResult Index()
        {
            return View();
        }

        //SignUp Form Returning Data
        [HttpPost]
        public IActionResult Index(Users NewUser)
        {

            bool IsUsernameExists = _UsersDB.Users.Any(UserObj => UserObj.UserName == NewUser.UserName);

            // Makes sure form was valid and primary key wasnt repeated
            if (ModelState.IsValid && !IsUsernameExists)
            {
                _UsersDB.Users.Add(NewUser);

                _UsersDB.SaveChanges();
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
    }
}
