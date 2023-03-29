using CI.DataAcess.Repository.IRepository;
using CI.Models;
using CI.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI.DataAcess.Repository
{
    public class Profile : IProfile
    {
        private readonly CiPlatformContext _db;

        public Profile(CiPlatformContext db)
        {
            _db = db;
        }
        public ProfileViewModel Get_Initial_Details(int country)
        {
            if (country == 0)
            {
                List<Country> countries = _db.Countries.ToList();
                List<Skill> skills = _db.Skills.ToList();
                List<City> cities = _db.Cities.ToList();
                return new ProfileViewModel { Countries = countries, Cities = cities, Skills = skills };
            }
            else
            {
                List<City> cities = _db.Cities.Where(c => c.CountryId == country).ToList();
                return new ProfileViewModel { Cities = cities };
            }
        }

        public bool Update_Profile(ProfileViewModel Details, long User_id)
        {
            User? user = _db.Users.FirstOrDefault(c => c.UserId == User_id);
            if (user != null)
            {
                user.FirstName = Details.user.FirstName;
                user.LastName = Details.user.LastName;
                user.Title = Details.user.Title;
                user.CountryId = Details.user.CountryId;
                user.WhyIVolunteer = Details.user.WhyIVolunteer;
                user.CityId = Details.user.CityId;
                user.LinkedInUrl = Details.user.LinkedInUrl;
                user.Availablity = Details.user.Availablity;
                user.ProfileText = Details.user.ProfileText;
                user.Department = Details.user.Department;
                user.UpdatedAt = DateTime.Now;


                if (Details.Selected_Skills is not null)
                {
                    List<UserSkill> user_Skills = _db.UserSkills.Where(c => c.UserId == User_id).ToList();
                    if (user_Skills.Count > 0)
                    {
                        _db.RemoveRange(user_Skills);
                        string[] skills = Details.Selected_Skills.Split(',');
                        foreach (string sk in skills)
                        {
                            _db.UserSkills.Add(new UserSkill { SkillId = int.Parse(sk), UserId = User_id });
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
