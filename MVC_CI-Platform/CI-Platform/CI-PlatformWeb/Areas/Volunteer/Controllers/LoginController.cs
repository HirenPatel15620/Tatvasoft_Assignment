using CI_Platform.Models;
using CI_Platform.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using CI_Platform.Models.Models;

namespace CI_PlatformWeb.Areas.Volunteer.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILoginRepository _loginRepository;

        public LoginController(ILoginRepository loginRepository)
        {
            _loginRepository = loginRepository;
        }

        public IActionResult Index(string ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            if (TempData["Logout"] != null)
                ViewBag.success = TempData["Logout"];
            if (TempData["Registration"] != null)
                ViewBag.success = TempData["Registration"];
            if (TempData["resetpass"] != null)
                ViewBag.success = TempData["resetpass"];
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(User user, string ReturnUrl)
        {
            var userdata = _loginRepository.getUserByEmail(user.Email);
            if (userdata != null)
            {
                if (userdata.Status == 1)
                {
                    bool isValid = (userdata.Email.Equals(user.Email) && userdata.Password.Equals(user.Password));
                    if (isValid)
                    {
                        var identity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Email, user.Email) },
                            CookieAuthenticationDefaults.AuthenticationScheme);
                        identity.AddClaim(new Claim(ClaimTypes.Name, userdata.FirstName));
                        identity.AddClaim(new Claim(ClaimTypes.Surname, userdata.LastName));
                        var principle = new ClaimsPrincipal(identity);
                        HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principle);
                        HttpContext.Session.SetString("Email", user.Email);
                        if (ReturnUrl != null)
                        {
                            var viewurl = ReturnUrl.Split('/');
                            return RedirectToAction(viewurl[3], viewurl[2], new { Area = viewurl[1] });
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home", new { Area = "Volunteer" });
                        }
                    }
                    else
                    {
                        ViewBag.error = "Your Password is Wrong!";
                    }
                }
                else
                {
                    ViewBag.error = "User is no active longer!";
                }
            }
            else
            {
                ViewBag.error = "Email Id not Found!";
            }
            return View();
        }

        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Registration(User user)
        {

            if (_loginRepository.getUserByEmail(user.Email) != null)
            {
                ViewBag.error = user.Email + " was already Registered!";
            }
            else if(_loginRepository.getUserByPhone(user.PhoneNumber) != null)
            {
                ViewBag.error = user.PhoneNumber + " was already Registered!";
            }
            else
            {
                _loginRepository.InsertUser(user);
                _loginRepository.Save();
                TempData["Registration"] = "User Registred Successfully! Please Login!";
                return RedirectToAction("Index", "Login", new { Area = "Volunteer" });
            }
            return View();
        }

        public IActionResult ResetPass(string email, string token)
        {
            var dataToken = _loginRepository.getTokenByEmail(email);
            if (dataToken != null)
                //if (dataToken.Used == 1)
                {
                var date1 = DateTime.Now;
                var date2 = date1.AddHours(-4);
                if (dataToken.Token1 == token && dataToken.GeneratedAt > date2 && dataToken.GeneratedAt < date1)
                {
                    ViewBag.email = email;
                    ViewBag.token = token;
                    return View();
                }
            }
            TempData["resetpass"] = "Something was changed in Url or Url was expired! Please try again!";
            return RedirectToAction("ForgotPass", "Login", new { Area = "Volunteer" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ResetPass(string email, string pass, string token)
        {
            var user = _loginRepository.getUserByEmail(email);
            if (user != null)
            {
                var dataToken = _loginRepository.getTokenByEmail(email);
                if (dataToken.Token1 == token)
                {
                    user.Password = pass;
                    user.UpdateAt = DateTime.Now;
                    dataToken.Used = 1;
                    _loginRepository.UpdateUser(user);
                    _loginRepository.UpdateToken(dataToken);
                    _loginRepository.Save();
                    TempData["resetpass"] = "Password is changed successfully!";
                    return RedirectToAction("Index", "Login", new { Area = "Volunteer" });
                }
            }
            ViewBag.error = "Something went Wrong! Please try again!";
            return View();
        }

        public IActionResult ForgotPass()
        {
            if (TempData["resetpass"] != null)
                ViewBag.error = TempData["resetpass"];
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ForgotPass(string email)
        {
            var userdata = _loginRepository.getUserByEmail(email);
            if (userdata != null)
            {
                var dataToken = _loginRepository.getTokenByEmail(email);
                string token = _loginRepository.TokenGenerate();
               Token emailtoken = new Token();
                if (dataToken != null)
                {
                    dataToken.Token1 = token;
                    dataToken.Used = 0;
                    dataToken.GeneratedAt = DateTime.Now;
                    _loginRepository.UpdateToken(dataToken);
                }
                else
                {
                    emailtoken.Email = email;
                    emailtoken.Token1 = token;
                    emailtoken.Used = 0;
                    emailtoken.GeneratedAt = DateTime.Now;
                    _loginRepository.InsertToken(emailtoken);
                }
                _loginRepository.Save();
                var link = Url.Action("ResetPass", "Login", new { Area = "Volunteer", email = email, token = token });
                var mailBody = "<h1>Reset Password Link:</h1><br> <a href='https://localhost:7275" + link + "'> <b style='color:red;'>Click Here to Forgot Password</b>  </a>";
                _loginRepository.SendMail(mailBody, email);
                ViewBag.success = "Mail sent Successfully! Plese check mail";
            }
            else
            {
                ViewBag.error = "Email Not Found";
            }
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            TempData["Logout"] = "Logout Successfully";
            return RedirectToAction("Index", "Home", new { Area = "Volunteer" });
        }
    }
}
