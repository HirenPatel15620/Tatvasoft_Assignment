using CI_platform.Entities.DataModels;
using Microsoft.AspNetCore.Mvc;

namespace CI_platform.Controllers
{
    public class UserAuthenticationController : Controller
    {
        private readonly CiPlatformDbContext _db;
        public UserAuthenticationController(CiPlatformDbContext db)
        {
            _db = db;
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(Admin user)
        {
            if (ModelState.IsValid)
            {
                _db.Admins.Add(user);
                _db.SaveChanges();
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
