using CI_Platform.Auth;
using CI_Platform.Entities.Auth;
using CI_Platform.Entities.Models;
using CI_Platform.Entities.ViewModels;
using CI_Platform.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CI_Platform.Controllers
{
    [Authorize(Roles = "User")]
    public class UserController : Controller
    {
        private readonly IUser _user;
        private readonly IConfiguration _configuration;

        public UserController(IUser user, IConfiguration configuration)
        {
            _user = user;
            _configuration = configuration;
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            ViewBag.Crousel = _user.GetCrouselImages();
            return View();
        }

        [AllowAnonymous]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction( "Login" , "User");
        }

        #region User Login by PostMethod
        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login(User obj)
        {
            var user = _user.login(obj);
            if (user == null)
            {
                ViewBag.Crousel = _user.GetCrouselImages();
                TempData["Message"] = "Invalid Email or Password";
                return View();

            }
            else
            {
                var jwtSettings = _configuration.GetSection(nameof(JwtSetting)).Get<JwtSetting>();
                var token = JwtTokenHelper.GenerateToken(jwtSettings, user);
                HttpContext.Session.SetString("Token", token);
                var returnUrl = HttpContext.Session.GetString("returnUrl");
                if (!string.IsNullOrEmpty(returnUrl))
                {
                    HttpContext.Session.Remove("returnUrl");
                    return Redirect(returnUrl);
                }
                if (user.Role == "User")
                    return RedirectToAction("Platform", "Platform");
                else
                    return RedirectToAction("AdminPanel", "Admin");
            }
        }
        #endregion

        [AllowAnonymous]
        public IActionResult Forgot()
        {
            ViewBag.Crousel = _user.GetCrouselImages();
            return View();
        }

        #region Forgot Password by User With PostMethod
        [AllowAnonymous]
        [HttpPost]
        public IActionResult Forgot(User obj)
        {
            ViewBag.Crousel = _user.GetCrouselImages();
            var user = _user.forgot(obj);
            if (user == null)
            {
                TempData["Message"] = "Invalid Email";
                return View();
            }
            TempData["Message"] = "Check your email to reset password";
            return RedirectToAction("Login");
        }
        #endregion

        [AllowAnonymous]
        public IActionResult Reset()
        {
            ViewBag.Crousel = _user.GetCrouselImages();
            return View();
        }

        #region Reset Password by User With PostMethod
        [AllowAnonymous]
        [HttpPost]
        public IActionResult Reset(User obj, string token)
        {
            ViewBag.Crousel = _user.GetCrouselImages();
            var validToken = _user.reset(obj, token);
            if (validToken != null)
            {
                TempData["Message"] = "Your Password is changed";
                return RedirectToAction("Login");
            }
            TempData["Message"] = "Token not Found";
            return RedirectToAction("Login");
        }
        #endregion

        [AllowAnonymous]
        public IActionResult Register()
        {
            ViewBag.Crousel = _user.GetCrouselImages();
            return View();
        }

        #region Register User With PostMethod
        [AllowAnonymous]
        [HttpPost]
        public IActionResult Register(User obj)
        {
            var user = _user.register(obj);
            ViewBag.Crousel = _user.GetCrouselImages();

            if (user != null)
            {
                TempData["Message"] = "User Already Exists";
                return RedirectToAction("Register");
                
            }
            TempData["Message"] = "You are Successfully Registered";
            return RedirectToAction("Login");
        }
        #endregion

        #region Get UserProfile
        [AllowAnonymous]
        public IActionResult UserProfile() 
        {
            var useridClaim = HttpContext.User?.FindFirst("UserId");
            if (useridClaim != null)
            {
                int userid = JsonSerializer.Deserialize<int>(useridClaim.Value);

                var userDetails = _user.GetUserProfile(userid);
                return View(userDetails);
            }
            else
            {
                TempData["Message"] = "Login is Required";
                return RedirectToAction("Login", "User");
            }
        }
        #endregion

        [AllowAnonymous]
        public JsonResult GetCityListbyCountry(int countryid)
        {
            var cityList =_user.GetCityListbyCountry(countryid);
            return Json(cityList);
        }

        #region Change Password by User
        [AllowAnonymous]
        public bool  ChangeUserPassword(string oldpass, string newpass)
        {
            var useridClaim = HttpContext.User?.FindFirst("UserId");
            if (useridClaim != null)
            {
                int userId = JsonSerializer.Deserialize<int>(useridClaim.Value);
                var check = _user.ChangeUserPassword(userId, oldpass, newpass);
                return check;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region Update UserProfile by User
        [AllowAnonymous]
        public bool UserProfileUpdate(UserDetailsModel model)
        {
            var useridClaim = HttpContext.User?.FindFirst("UserId");
            if (useridClaim != null)
            {
                int userId = JsonSerializer.Deserialize<int>(useridClaim.Value);
                var check = _user.UserProfileUpdate(model,userId);
                HttpContext.Session.SetString("Uname", model.FirstName + " " + model.LastName);
                if (!string.IsNullOrEmpty(model.AvatarName)) HttpContext.Session.SetString("avtar", model.AvatarName);
                return check;
            }
            else
            {
                return false;
            }

        }
        #endregion

        [AllowAnonymous]
        public void ContactUs(ContactU model)
        {
            var useridClaim = HttpContext.User?.FindFirst("UserId");
            if (useridClaim != null)
            {
                int userId = JsonSerializer.Deserialize<int>(useridClaim.Value);
                _user.ContactUs(userId, model);
            }
        }

        [AllowAnonymous]
        public IActionResult Privacy()
        {
            var privacy = _user.GetPrivacyData();
            return View(privacy);
        }

    }
}
