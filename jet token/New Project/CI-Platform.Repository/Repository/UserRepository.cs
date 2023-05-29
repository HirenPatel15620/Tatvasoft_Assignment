using CI_Platform.Entities.Data;
using CI_Platform.Entities.Models;
using CI_Platform.Repository.Interface;
using MailKit.Security;
using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;
using CI_Platform.Entities.ViewModels;
using System.Web.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Web.Helpers;

namespace CI_Platform.Repository.Repository
{
    public class UserRepository  : IUser
    {
        private readonly CiPlatformContext _ciPlatformContext;
        public UserRepository(CiPlatformContext ciPlatformContext)
        {
            _ciPlatformContext = ciPlatformContext;
        }

        #region User Login
        public User login(User obj)
        {
            var user = _ciPlatformContext.Users.FirstOrDefault(x => x.Email == obj.Email  && x.DeletedAt == null);
            if (user != null && Crypto.VerifyHashedPassword(user.Password, obj.Password))
            {
                return user;
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region Forgot Password 
        public User forgot(User obj)
        {
            var user = _ciPlatformContext.Users.FirstOrDefault(u => u.Email.Equals(obj.Email.ToLower()) && u.DeletedAt == null);

            if(user == null)
            {
                return null;
            }

            #region Genrate Token
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[16];
            var random = new Random();
            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }
            var finalString = new String(stringChars);
            #endregion Genrate Token

            #region Update Password Reset Table
            PasswordReset entry = new PasswordReset();
            entry.Email = obj.Email;
            entry.Token = finalString;
            _ciPlatformContext.PasswordResets.Add(entry);
            _ciPlatformContext.SaveChanges();
            #endregion Update Password Reset Table

            #region Send Mail
            var mailBody = "<h1>Click link to reset password</h1><br><h2><a href='" + "https://localhost:44356/User/Reset?token=" + finalString + "'>Reset Password</a></h2>";

            // create email message
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("devloper.testing2022@gmail.com"));
            email.To.Add(MailboxAddress.Parse(user.Email));
            email.Subject = "Reset Your Password";
            email.Body = new TextPart(TextFormat.Html) { Text = mailBody };

            // send email
            using var smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("devloper.testing2022@gmail.com", "zryemtpwhipptczr");
            smtp.Send(email);
            smtp.Disconnect(true);
            #endregion Send Mail

            return user;
        }
        #endregion

        #region Reset Password by User
        public PasswordReset reset(User obj, string token)
        {
            var validToken = _ciPlatformContext.PasswordResets.FirstOrDefault(x => x.Token == token);
            if (validToken != null)
            {
                var user = _ciPlatformContext.Users.FirstOrDefault(x => x.Email == validToken.Email && x.DeletedAt == null); 
                user.Password = Crypto.HashPassword(obj.Password);
                _ciPlatformContext.Users.Update(user);
                _ciPlatformContext.SaveChanges();
                
            }          
            return validToken;
        }
        #endregion

        #region Register by User
        public User register(User obj)
        {
            var user = _ciPlatformContext.Users.FirstOrDefault( x => x.Email == obj.Email && x.DeletedAt == null);

            if (user == null)
            {
                User userRegister = new User();

                userRegister.FirstName = obj.FirstName;
                userRegister.LastName = obj.LastName;
                userRegister.Email = obj.Email;
                userRegister.Password = Crypto.HashPassword(obj.Password);
                userRegister.PhoneNumber = obj.PhoneNumber;
                userRegister.Role = "User";

                _ciPlatformContext.Users.Add(userRegister);
                _ciPlatformContext.SaveChanges();
            }

            return user;    
        }
        #endregion

        #region Get User Data For UserProfile Page
        public UserProfileModel GetUserProfile(int userid) 
        {
            var user = _ciPlatformContext.Users.FirstOrDefault(user => user.UserId == userid);
            var userSkills = _ciPlatformContext.UserSkills.Where(uskill => uskill.UserId == userid).Select(x => x.SkillId);
            UserProfileModel model = new UserProfileModel();
            
                model.cityList = _ciPlatformContext.Cities.Where(city => city.CountryId == (user.CountryId ?? 0)).Select(
                    city => new SelectListItem
                    {
                        Text = city.Name,
                        Value = city.CityId.ToString(),
                        Selected = city.CityId == user.CityId
                    }).ToList();
           
            model.countryList = _ciPlatformContext.Countries.Select(
                country => new SelectListItem
                {
                    Text = country.Name,
                    Value = country.CountryId.ToString(),
                    Selected = country.CountryId == user.CountryId
                }
                ).ToList();
            model.skillList = _ciPlatformContext.Skills.Select( skill => new SelectListItem
            {
                Text = skill.SkillName,
                Value = skill.SkillId.ToString(),
                Selected = userSkills.Contains(skill.SkillId)
            }).ToList();
            model.userProfile = user;

            return model;
        }
        #endregion

        #region City by Country For Add User City
        public UserProfileModel GetCityListbyCountry(int countryid)
        {
            UserProfileModel model = new UserProfileModel();
            model.cityList = _ciPlatformContext.Cities.Where(city => city.CountryId == countryid).Select(
                    city => new SelectListItem
                    {
                        Text = city.Name,
                        Value = city.CityId.ToString(),
                        
                    }).ToList();
            return model;
        }
        #endregion

        #region Change Password by User
        public bool ChangeUserPassword(int userid, string oldpass, string newpass)
        {
            var user = _ciPlatformContext.Users.FirstOrDefault(x => x.UserId == userid);
            if(Crypto.VerifyHashedPassword(user.Password , oldpass))
            {
                user.Password = Crypto.HashPassword(newpass);
                _ciPlatformContext.Users.Update(user);
                _ciPlatformContext.SaveChanges();   
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region UserProfile CRUD by User
        public bool UserProfileUpdate(UserDetailsModel model , int userid)
        {
            var user = _ciPlatformContext.Users.Include(x => x.UserSkills).FirstOrDefault(x => x.UserId == userid && x.DeletedAt == null);
            if(user != null)
            {
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.EmployeeId = model.EmployeeId;
                user.CountryId = model.CountryId;
                user.CityId = model.CityId;
                user.Department = model.Department;
                user.WhyIVolunteer = model.WhyIVolunteer;
                user.Title = model.Title;
                user.Status = model.Status;
                user.ProfileText = model.ProfileText;
                user.LinkedInUrl = model.LinkedInUrl;
                user.UpdatedAt = DateTime.Now;
               
                if(model.Avatar != null)
                {
                    if(user.Avatar != null)
                    {
                        string delImagwe = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\media", user.Avatar);
                        if (File.Exists(delImagwe))
                        {
                            File.Delete(delImagwe);
                        }
                    }

                    string avtarName = Guid.NewGuid().ToString() + Path.GetExtension(model.Avatar.FileName);
                    string uploadAvtarPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\media", avtarName);

                    using (var filestream = new FileStream(uploadAvtarPath, FileMode.Create))
                    {
                        model.Avatar.CopyTo(filestream);
                    }
                    user.Avatar = avtarName;
                    model.AvatarName = avtarName;
                }
                

                if(model.SkillIds != null)
                {
                    var userSkills = user.UserSkills.Where(x => x.DeletedAt == null);
                    var addUserSkills = model.SkillIds.Except(userSkills.Select(x => x.SkillId)).ToList();
                    var removeUserSkills = userSkills.Where(x => !model.SkillIds.Contains(x.SkillId));
                    foreach(var skill in addUserSkills)
                    {
                        var uSkill = new UserSkill { UserId = userid, SkillId = skill };
                        _ciPlatformContext.UserSkills.Add(uSkill);
                    }
                    foreach(var skill in removeUserSkills)
                    {
                        _ciPlatformContext.UserSkills.Remove(skill);
                    }
                    _ciPlatformContext.SaveChanges();
                }

                _ciPlatformContext.Users.Update(user);
                _ciPlatformContext.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }

        }
        #endregion

        #region Contact US by User
        public void ContactUs(int userid, ContactU model)
        {
            ContactU contact = new ContactU();
            contact.UserId = userid;
            contact.Subject = model.Subject;
            contact.Message = model.Message;
            _ciPlatformContext.ContactUs.Add(contact);
            _ciPlatformContext.SaveChanges();
        }
        #endregion

        #region Privacy Policy Data For Privacy Page
        public List<CmsPage> GetPrivacyData()
        {
            var privacy = _ciPlatformContext.CmsPages.Where(cms => cms.DeletedAt == null && cms.Status == 1).ToList();
            if (privacy == null)
                return new List<CmsPage>();
            
            return privacy;
        }
        #endregion

        #region Crousel Images For Login Page
        public List<Banner> GetCrouselImages()
        {
            var banner = _ciPlatformContext.Banners.OrderBy(b => b.SortOrder).Where(b => b.DeletedAt == null).ToList();
            return banner;
        }
        #endregion
    }
}
