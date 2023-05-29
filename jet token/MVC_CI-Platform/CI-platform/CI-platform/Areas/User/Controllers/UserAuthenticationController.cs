using CI.Models;
using Microsoft.AspNetCore.Mvc;
using CI.Repository.Repository.IRepository;
using CI.Models.ViewModels;
using System.Net.Mail;
using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using CI_platform.helper;



namespace CI_platform.Controllers
{
    [Area("User")]
    public class UserAuthenticationController : Controller
    {
        private readonly IAllRepository allRepository;
        private readonly IConfiguration configuration;

        public UserAuthenticationController(IAllRepository _allRepository, IConfiguration _configuration)
        {
            allRepository = _allRepository;
            configuration = _configuration;

        }
        [AllowAnonymous]
        [Route("Logout")]
        public async Task<IActionResult> Logout()
        {
           
                //await HttpContext.SignOutAsync("AuthCookie");
                ViewBag.banner = allRepository.AdminStory.GetAllBanners().BannerList;
                HttpContext.Session.Clear();
                return RedirectToAction("Login");
            
          
        }

        [AllowAnonymous]
        public IActionResult Login(string? returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {

                if (returnUrl is not null)
                {
                    return new RedirectResult(returnUrl);
                }
                return RedirectToAction("home", "home");
            }
            else
            {
                if (returnUrl is not null)
                {
                    TempData["returnUrl"] = returnUrl;
                }
                ViewBag.banner = allRepository.AdminStory.GetAllBanners().BannerList;
                
                return View();
            }
        }




        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(Login login)
        {
            var Logintoken = HttpContext.Request.Cookies["JWTToken"]?.ToString();
            if (Logintoken != null)
            {
                var principles = JwtTokenHelper.ValidateJwtToken(Logintoken);
                //var principles = JwtTokenHelper.GenerateToken(Logintoken);

                if (principles != null)
                {
                    if (principles.Identity.IsAuthenticated)
                    {
                        return RedirectToAction("home", "home");
                    }
                }
            }
          

            if (ModelState.IsValid)
            {
                User check = allRepository.UserAuthentication.GetFirstOrDefault(c => c.Email.Equals(login.Email) && c.Status=="1") ;

                if (check == null)
                {
                    ViewData["login"] = "register";
                    ViewBag.banner = allRepository.AdminStory.GetAllBanners().BannerList;
                    return View();
                }
                else
                {
                    bool verify = BCrypt.Net.BCrypt.Verify(login.Password, check.Password);

                    ViewData["login"] = "mainpage";
                    if (verify is true)
                    {
                        if (check.Role is not null)
                        {
                            HttpContext.Session.SetString("role", check?.Role);

                        }

                        if (check.Avatar is not null)
                        {

                          
                            //var identity = new ClaimsIdentity(claims, "AuthCookie");
                            //var Principle = new ClaimsPrincipal(identity);
                            HttpContext.Session.SetString("Avatar", check.Avatar);

                            HttpContext.Session.SetString("Country", check.CountryId.ToString());
                            HttpContext.Session.SetString("City", check.CityId.ToString());
                            //await HttpContext.SignInAsync("AuthCookie", Principle);
                            //if (check.Role is not null)
                            //{
                            //    HttpContext.Session.SetString("role", check?.Role);
                            //    return RedirectToAction("Index", "User", new { area = "Admin" });
                            //}
                            //if (TempData.ContainsKey("returnUrl"))
                            //{
                            //    var url = TempData["returnUrl"] as string;
                            //    return new RedirectResult(url);
                            //}





                            //this is temp/////////////////////////
                            var jwtSettings = configuration.GetSection(nameof(JwtSetting)).Get<JwtSetting>();
                            var token = JwtTokenHelper.GenerateToken(jwtSettings, check);
                            HttpContext.Session.SetString("Token", token);
                            HttpContext.Response.Cookies.Append("JWTToken", token, new CookieOptions { HttpOnly = true, Secure = true, Expires = DateTime.Now.AddMinutes(20) });
                            var returnUrl = HttpContext.Session.GetString("returnUrl");
                            if (!string.IsNullOrEmpty(returnUrl))
                            {
                                HttpContext.Session.Remove("returnUrl");
                                return Redirect(returnUrl);
                            }
                            if (check.Role is not null)
                            {
                                HttpContext.Session.SetString("role", check?.Role);
                                return RedirectToAction("Index", "User", new { area = "Admin" });
                            }
                            if (TempData.ContainsKey("returnUrl"))
                            {
                                var url = TempData["returnUrl"] as string;
                                return new RedirectResult(url);
                            }
                            //await HttpContext.SignInAsync(token);
                          

                            return RedirectToAction("home", "home");

                        }

                        else
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
                            return RedirectToAction("Profile", "Home");
                        }

                    }


                    else
                    {
                        ViewData["login"] = "password";
                        ViewBag.banner = allRepository.AdminStory.GetAllBanners().BannerList;
                        return View();
                    }

                }
            }
            ViewBag.banner = allRepository.AdminStory.GetAllBanners().BannerList;
            return View();
        }


        [AllowAnonymous]
        [Route("Register")]
        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("home", "home");
            }
            else
            {
                ViewBag.banner = allRepository.AdminStory.GetAllBanners().BannerList;
                return View();
            }
        }
        [Route("Register")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Register(Register user)
        {
            if (ModelState.IsValid)
            {
                var check = allRepository.UserAuthentication.GetFirstOrDefault(c => c.Email.Equals(user.Email.ToLower()));
                if (check == null)
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

                    }
                    NotificationSetting notificationSetting = new NotificationSetting();
                    {
                        notificationSetting.ApplicationApprove = 1;
                        notificationSetting.StoryApprove = 1;
                        notificationSetting.NewMission = 1;
                        notificationSetting.RecommendMission = 1;
                        notificationSetting.RecommendStory = 1;
                        notificationSetting.FromMail = 1;
                        notificationSetting.NewMessage = 1;
                        newuser.NotificationSettings.Add(notificationSetting);
                        //notification.UserNotifications.Add(userNotification);
                    }
                    allRepository.UserAuthentication.Add(newuser);
                    allRepository.save();
                    check = allRepository.UserAuthentication.GetFirstOrDefault(c => c.Email.Equals(user.Email.ToLower()));
                    var claims = new List<Claim>
                            {
                                  new Claim(ClaimTypes.Name, $"{check.FirstName} {check.LastName}"),
                                  new Claim(ClaimTypes.Email, check.Email),
                                  new Claim(ClaimTypes.Sid, check.UserId.ToString()),
                               };
                    var identity = new ClaimsIdentity(claims, "AuthCookie");
                    var Principle = new ClaimsPrincipal(identity);
                    await HttpContext.SignInAsync("AuthCookie", Principle);
                    return RedirectToAction("Profile", "Home");
                }
                else
                {
                    ViewData["register"] = "warning";
                    ViewBag.banner = allRepository.AdminStory.GetAllBanners().BannerList;
                    return View();
                }
            }
            ViewBag.banner = allRepository.AdminStory.GetAllBanners().BannerList;
            return View();


        }
        [AllowAnonymous]
        [Route("ResetPassword")]
        public IActionResult ResetPassword(string token,string Email)
        {
            if (TempData.Peek("email") is not null)
            {

                ViewData["ResetPassword"] = "EmailSent";
                ViewBag.banner = allRepository.AdminStory.GetAllBanners().BannerList;
                return View();
            }

            var check = allRepository.ResetPassword.GetFirstOrDefault(c => c.Email.Equals(Email));
            if (check is not null)
            {

                var mail = allRepository.ResetPassword.GetFirstOrDefault(c => c.Email.Equals(Email));
                string email = mail.Email;
                string Token = mail.Token;
                //PasswordReset data = db.ResetPassword.GetFirstOrDefault(c => c.Email.Equals(email));

                if (Token != null)
                {
                    var date1 = DateTime.Now;
                    var date2 = date1.AddHours(-4);
                    if (mail.Token == token && mail.CreatedAt > date2 && mail.CreatedAt < date1)
                    {
                        ViewBag.email = mail.Email;
                        ViewBag.token = token;
                        ViewBag.banner = allRepository.AdminStory.GetAllBanners().BannerList;
                        return View();
                    }
                    if(mail.CreatedAt < date2 )
                    {
                        ViewData["ResetPassword"] = "LinkExpire";
                        ViewBag.banner = allRepository.AdminStory.GetAllBanners().BannerList;
                        return View();

                    }
                }
            }
            if (check is null)
            {
                ViewData["ResetPassword"] = "Link";
                ViewBag.banner = allRepository.AdminStory.GetAllBanners().BannerList;
                return View();
            }

            ViewBag.banner = allRepository.AdminStory.GetAllBanners().BannerList;
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [Route("ResetPassword")]
        public IActionResult ResetPassword(ResetPassword pass)
        {
            if (ModelState.IsValid)
            {
                if (pass.Email is not null)
                {
                    string email = pass.Email;
                    PasswordReset data = allRepository.ResetPassword.GetFirstOrDefault(c => c.Email.Equals(email));
                    string Token = data.Token;


                    if (data is null)
                    {
                        ViewData["ResetPassword"] = "token";
                        return View();

                    }

                    if (data.Token == pass.Token)
                    {
                        string passtoken = BCrypt.Net.BCrypt.HashPassword(pass.Password);
                        User myuser = allRepository.UserAuthentication.ResetPassword(passtoken, email);
                        if (myuser == null)
                        {
                            ViewData["ResetPassword"] = "false";
                            ViewBag.banner = allRepository.AdminStory.GetAllBanners().BannerList;
                            return View();
                        }
                        else
                        {
                            allRepository.save();
                            ViewBag.banner = allRepository.AdminStory.GetAllBanners().BannerList;
                            return RedirectToAction("Login");
                        }
                    }
                    else
                    {
                        ViewData["ResetPassword"] = "false";
                        ViewBag.banner = allRepository.AdminStory.GetAllBanners().BannerList;
                        return View();
                    }
                }
                else
                {
                    ViewData["ResetPassword"] = "false";
                    ViewBag.banner = allRepository.AdminStory.GetAllBanners().BannerList;
                    return View();
                }
            }
            else
            {
                ViewData["ResetPassword"] = "false";
                ViewBag.banner = allRepository.AdminStory.GetAllBanners().BannerList;
                return View();
            }
        }
        [AllowAnonymous]
        [Route("ForgotPassword")]
        public IActionResult ForgotPassword()
        {
            ViewBag.banner = allRepository.AdminStory.GetAllBanners().BannerList;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("ForgotPassword")]
        public IActionResult ForgotPassword(ForgotPassword user)

        {


            if (ModelState.IsValid)
            {
                User myuser = allRepository.UserAuthentication.GetFirstOrDefault(c => c.Email.Equals(user.Email.ToLower()));


                if (myuser == null)
                {
                    ViewBag.banner = allRepository.AdminStory.GetAllBanners().BannerList;
                    return View();
                }
                else
                {
                    string token = BCrypt.Net.BCrypt.HashString(user.Email.ToLower().ToString());
                    TempData["email"] = user.Email;
                    var link = Url.Action("ResetPassword", "UserAuthentication", new { email = user.Email, token = token });
                    var senderEmail = new MailAddress("tatvasoft51@gmail.com", "CI-Platform");
                    var receiverEmail = new MailAddress(user.Email, "Receiver");
                    var password = "vlpzyhibrvpaewte";
                    var sub = "Reset Your Password";
                    //var body = "Your Reset Password Token : " + link;
                    var mailBody = "<h1>Reset Password Link:</h1><br> <a href='https://localhost:7180" + link + "'> <b style='color:red;'>Click Here to Forgot Password</b>  </a>";
                   
                    //var body = "Your Reset Password Token : " + token;
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
                        Body = mailBody,
                        IsBodyHtml = true
                    })
                    {
                        smtp.Send(mess);
                    }
                    PasswordReset passwordReset = new PasswordReset()
                    {
                        Email = user.Email,
                        Token = token
                    };
                    PasswordReset data = allRepository.ResetPassword.GetFirstOrDefault(c => c.Email.Equals(user.Email));
                    if (data is null)
                    {
                        allRepository.ResetPassword.Add(passwordReset);
                        allRepository.save();
                    }
                    else
                    {
                        allRepository.ResetPassword.DeleteData(data);
                        allRepository.ResetPassword.Add(passwordReset);
                        allRepository.save();
                    }
                    return RedirectToAction("ResetPassword");

                }
            }
            ViewBag.banner = allRepository.AdminStory.GetAllBanners().BannerList;
            return View();
        }
    }
}
