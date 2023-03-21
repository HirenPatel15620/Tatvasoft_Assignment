using CI.Models;
using Microsoft.AspNetCore.Mvc;
using CI.DataAcess.Repository.IRepository;
using CI.Models.ViewModels;
using System.Net.Mail;
using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace CI_platform.Controllers
{ [Area("User")]
    public class UserAuthenticationController : Controller
    {
        private readonly IAllRepository db;
        public UserAuthenticationController(IAllRepository _db)
        {
            db = _db;
        }
        [Route("Logout")]
        public async Task<IActionResult> Logout()
        {
            if (User.Identity.IsAuthenticated)
            {
                await HttpContext.SignOutAsync("AuthCookie");
                return RedirectToAction("Login");
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("home", "home");
            }
            else
            {
                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> Login(Login login)
        {
            if (ModelState.IsValid)
            {
                User check = db.UserAuthentication.GetFirstOrDefault(c => c.Email.Equals(login.Email));
                if (check == null)
                {
                    ViewData["login"] = "register";
                    return View();
                }
                else
                {
                    bool verify = BCrypt.Net.BCrypt.Verify(login.Password, check.Password);
                    if (verify is true)
                    {
                        var claims = new List<Claim>
                            {
                                  new Claim(ClaimTypes.Name, $"{check.FirstName} {check.LastName}"),
                                  new Claim(ClaimTypes.Email, check.Email),
                                  new Claim(ClaimTypes.Sid, check.UserId.ToString()),
                               };
                        var identity = new ClaimsIdentity(claims, "AuthCookie");
                        var Principle = new ClaimsPrincipal(identity);

                        await HttpContext.SignInAsync("AuthCookie", Principle);
                        return RedirectToAction("home", "home");
                    }
                    else
                    {
                        ViewData["login"] = "password";
                        return View();
                    }

                }
            }
            return View();
        }

        [Route("Register")]
        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("home", "home");
            }
            else
            {
                return View();
            }
        }
        [Route("Register")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Register user)
        {
            if (ModelState.IsValid)
            {
                var check = db.UserAuthentication.GetFirstOrDefault(c => c.Email.Equals(user.Email.ToLower()));
                   var bcheck = db.UserAuthentication.GetFirstOrDefault(e => e.PhoneNumber.Equals(user.PhoneNumber));
                if (check == null && bcheck == null)
                {
                    string secpass = BCrypt.Net.BCrypt.HashPassword(user.Password);
                    User newuser = new User();
                    {
                        newuser.Email = user.Email;
                        newuser.FirstName = user.FirstName;
                        newuser.LastName = user.LastName;
                        newuser.Password = secpass;
                        newuser.PhoneNumber = user.PhoneNumber;
                        newuser.CreatedAt = DateTime.Now;
                        //newuser.CreatedAt = DateTime.Now.ToString("mm/dd/yyyy");
                    }
                    db.UserAuthentication.Add(newuser);
                    db.save();
                    check= db.UserAuthentication.GetFirstOrDefault(c => c.Email.Equals(user.Email.ToLower()));
                    var claims = new List<Claim>
                            {
                                  new Claim(ClaimTypes.Name, $"{check.FirstName} {check.LastName}"),
                                  new Claim(ClaimTypes.Email, check.Email),
                                  new Claim(ClaimTypes.Sid, check.UserId.ToString()),
                               };
                    var identity = new ClaimsIdentity(claims, "AuthCookie");
                    var Principle = new ClaimsPrincipal(identity);
                    await HttpContext.SignInAsync("AuthCookie", Principle);
                    return RedirectToAction("home", "Home");
                }
                else
                {
                    ViewData["register"] = "warning";
                    return View();
                }
            }
            return View();


        }
        [Route("ResetPassword")]
        public IActionResult ResetPassword()
        {
            if (TempData.Peek("email") is not null)
            {
                ViewData["ResetPassword"] = "EmailSent";
                return View();
            }
            return View();
        }
        [HttpPost]
        [Route("ResetPassword")]
        public IActionResult ResetPassword(ResetPassword pass)
        {
            if (ModelState.IsValid)
            {
                if (TempData.Peek("email") is not null)
                {
                    string email = TempData.Peek("email").ToString();
                    PasswordReset data = db.ResetPassword.GetFirstOrDefault(c => c.Email.Equals(email));
                    if (data.Token == pass.Token)
                    {
                        string passtoken = BCrypt.Net.BCrypt.HashPassword(pass.Password);
                        User myuser = db.UserAuthentication.ResetPassword(passtoken, email);
                        if (myuser == null)
                        {
                            ViewData["ResetPassword"] = "false";
                            return View();
                        }
                        else
                        {
                            db.save();
                            return RedirectToAction("Login");
                        }
                    }
                    else
                    {
                        ViewData["ResetPassword"] = "false";
                        return View();
                    }
                }
                else
                {
                    ViewData["ResetPassword"] = "false";
                    return View();
                }
            }
            else
            {
                ViewData["ResetPassword"] = "false";
                return View();
            }
        }
        [Route("ForgotPassword")]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        [Route("ForgotPassword")]
        public IActionResult ForgotPassword(ForgotPassword user)
            
        {
            if (ModelState.IsValid)
            {
                User myuser = db.UserAuthentication.GetFirstOrDefault(c => c.Email.Equals(user.Email.ToLower()));
                if (myuser == null)
                {
                    return View();
                }
                else
                {
                    string token = BCrypt.Net.BCrypt.HashString(user.Email.ToLower().ToString());
                    TempData["email"]=user.Email;
                    var senderEmail = new MailAddress("tatvasoft41@gmail.com", "Tatvasoft");
                    var receiverEmail = new MailAddress(user.Email, "Receiver");
                    var password = "vlpzyhibrvpaewte";
                    var sub = "Reset Your Password";
                    var body = "Your Reset Password Token" + token;
                    var smtp = new SmtpClient
                    {
                        Host = "smtp.gmail.com",
                        Port = 587,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(senderEmail.Address, password)
                    };
                    using (var mess = new MailMessage(senderEmail, receiverEmail)
                    {
                        Subject = sub,
                        Body = body
                    })
                    {
                        smtp.Send(mess);
                    }
                    PasswordReset passwordReset = new PasswordReset()
                    {
                        Email = user.Email,
                        Token = token
                    };
                    PasswordReset data = db.ResetPassword.GetFirstOrDefault(c => c.Email.Equals(user.Email));
                    if(data is null)
                    {
                        db.ResetPassword.Add(passwordReset);
                        db.save();
                    }
                    else
                    {
                        db.ResetPassword.DeleteData(data);
                        db.ResetPassword.Add(passwordReset);
                        db.save();
                    }
                    return RedirectToAction("ResetPassword");

                }
            }
            return View();
        }
    }
}
