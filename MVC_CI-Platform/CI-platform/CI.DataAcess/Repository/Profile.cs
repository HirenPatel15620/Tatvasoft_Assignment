using CI.Repository.Repository.IRepository;
using CI.Models;
using CI.Models.ViewModels;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;

namespace CI.Repository.Repository
{
    public class Profile : IProfile
    {
        private readonly CiPlatformContext _db;

        public Profile(CiPlatformContext db)
        {
            _db = db;
        }


     

        public bool Change_Password(string oldpassword, string newpassword, long User_id)
        {
            User user = _db.Users.FirstOrDefault(c => c.UserId == User_id);
            if (user is not null)
            {
                bool verify = BCrypt.Net.BCrypt.Verify(oldpassword, user.Password);
                if (verify)
                {
                    user.Password = BCrypt.Net.BCrypt.HashPassword(newpassword);
                    _db.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

        }

        public bool contactus(string subject, string message, long User_id)
        {
            User user = _db.Users.FirstOrDefault(c => c.UserId == User_id);
            if (user is not null)
            {
                ContactU contact = new ContactU()
                {
                    Subject = subject,
                    Message = message,
                    Email = user.Email,
                    UserId = User_id

                };
                _db.ContactUs.Add(contact);
                _db.SaveChanges();
                return true;
            }
            else { return false; }

        }

        public ProfileViewModel Get_Initial_Details(int country, long User_id)
        {
            User? user = _db.Users.FirstOrDefault(c => c.UserId == User_id);

            EditUser editUser = new EditUser()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Availablity = user.Availablity,
                Title = user.Title,
                Manager = user.Manager,
                CountryId = user.CountryId,
                CityId = user.CityId,
                Department = user.Department,
                EmployeeId = user.EmployeeId,
                ProfileText = user.ProfileText,
                LinkedInUrl = user.LinkedInUrl,
                WhyIVolunteer = user.WhyIVolunteer,
                UserId = user.UserId,
                role = user.Role,
            };


            if (country == 0)
            {

                List<Country> countries = _db.Countries.ToList();
                List<Skill> skills = _db.Skills.ToList();
                List<City> cities = _db.Cities.ToList();
                return new ProfileViewModel { Countries = countries, Cities = cities, Skills = skills, user = editUser };
            }
            else
            {
                List<City> cities = _db.Cities.Where(c => c.CountryId == country).ToList();
                return new ProfileViewModel { Cities = cities, user = editUser };
            }
        }

        public bool Update_Details(ProfileViewModel Details, long User_id)
        {
            User? user = _db.Users.FirstOrDefault(c => c.UserId == User_id);
            if (user is not null)
            {
                user.FirstName = Details.user.FirstName;
                user.LastName = Details.user.LastName;
                user.Title = Details.user.Title;
                user.Manager = Details.user.Manager;
                user.WhyIVolunteer = Details.user.WhyIVolunteer;
                user.Availablity = Details.user.Availablity;
                user.ProfileText = Details.user.ProfileText;
                user.LinkedInUrl = Details.user.LinkedInUrl;
                user.Department = Details.user.Department;
                user.CityId = Details.user.CityId;
                user.CountryId = Details.user.CountryId;
                user.UpdatedAt = DateTime.Now;
                user.EmployeeId = Details.user.EmployeeId;



                if (_db.Users.Any(u => u.EmployeeId == Details.EmployeeId && u.UserId != Details.user.UserId))
                {
                 
                    return false;
                }


                if (Details.profile is not null)
                {
                    using (var stream = Details.profile?.OpenReadStream())
                    {
                        var bytes = new byte[Details.profile.Length];
                        stream.Read(bytes, 0, (int)Details.profile.Length);
                        var base64string = Convert.ToBase64String(bytes);
                        user.Avatar = "data:image/png;base64," + base64string;
                    }
                }
                if (Details.Selected_Skills is not null)
                {
                    List<UserSkill> user_skills = _db.UserSkills.Where(c => c.UserId == User_id).ToList();
                    if (user_skills.Count > 0)
                    {
                        _db.RemoveRange(user_skills);
                        string[] skills = Details.Selected_Skills.Split(',');
                        foreach (var skill in skills)
                        {
                            _db.UserSkills.Add(new UserSkill { SkillId = int.Parse(skill), UserId = User_id });
                        }
                    }
                    else
                    {
                        string[] skills = Details.Selected_Skills.Split(',');
                        foreach (var skill in skills)
                        {
                            _db.UserSkills.Add(new UserSkill { SkillId = int.Parse(skill), UserId = User_id });
                        }
                    }
                }
                _db.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}

