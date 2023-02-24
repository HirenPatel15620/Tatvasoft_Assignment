using CI_platform.Models;
using Microsoft.AspNetCore.Mvc;

namespace CI_platform.Controllers
{
    
    public class UserAuthenticationController : Controller
    {
        private readonly CiPlatformContext _db;
        public UserAuthenticationController(CiPlatformContext db)
        {
            _db= db;
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(User user)
        {
            if (ModelState.IsValid)
            {
                return View();
            }
            return View();
        }
        [Route("Register")]
        public IActionResult Register()
        {
            return View();
        }
        [Route("Register")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                _db.Users.Add(user);
                _db.SaveChanges();

                
            }
            return View();

        }
        [Route("ResetPassword")]
        public IActionResult ResetPassword()
        {
            return View();
        }
        [HttpPost]
        [Route("ResetPassword")]
        public IActionResult ResetPassword(User user)
        {
            if (ModelState.IsValid)
            {
                return View();
            }
            return View();
        }
        [Route("ForgotPassword")]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        [Route("ForgotPassword")]
        public IActionResult ForgotPassword(User user)
        {
            if (ModelState.IsValid)
            {
                return View();
            }
            return View();
        }
    }
}
