using CI_Platform.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using System.Security.Claims;
using CI_Platform.Models.Data;
using CI_Platform.Models.Models;
using System.Text;

namespace CI_PlatformWeb.Areas.Volunteer.Controllers
{
    public class LoginController : Controller
    {
        private readonly CiPlatformContext _db;

        public LoginController(CiPlatformContext db)
        {
            _db = db;
        }
        public IActionResult Index(string ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(User user, string ReturnUrl)
        {
            var userdata = _db.Users.Where(x => x.Email == user.Email).SingleOrDefault();
            if (userdata != null)
            {
                if (userdata.Status == 0)
                {
                    bool isValid = (userdata.Email.Equals(user.Email) && userdata.Password.Equals(user.Password));
                    if (isValid)
                    {
                        var identity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, user.Email) },
                            CookieAuthenticationDefaults.AuthenticationScheme);
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
        public IActionResult ForgotPass()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ForgotPass(string email)
        {
            var data = _db.Users.Where(x => x.Email == email).SingleOrDefault();
            if (data != null)
            {
                var mailBody = "Your Password <b>" + data.Password + "</b>";
                Email(mailBody, email);
            }
            return View();
        }
        public IActionResult ResetPass()
        {
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
            if (_db.Users.Where(x => x.Email == user.Email).SingleOrDefault() == null)
            {
                _db.Users.Add(user);
                _db.SaveChanges();
            }
            else
            {
                ViewBag.error = user.Email + " was already Registered!";
            }
            return View();
        }
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home", new { Area = "Volunteer" });
        }


        //for sending email
        public static void Email(string body, string mailid)
        {
            try
            {
 

                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                message.From = new MailAddress("hirenpatel.ict19@gmail.com");
                message.To.Add(new MailAddress(mailid));
                message.Subject = "Your Password For CI-Platform";
                message.IsBodyHtml = true;
                message.Body = body;
                smtp.Port = 587;
                smtp.Host = "smtp.gmail.com";
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential("mailid", "password");
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(message);

            }
            catch (Exception) { }
        }
    }
}