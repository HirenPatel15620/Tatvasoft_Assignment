using CI.Models;
using CI.Repository.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI.Repository.Repository
{
    public class AdminMission : IAdminMission
    {
        private readonly CiPlatformContext _db;
        List<CI.Models.Mission> missions;
        List<CI.Models.MissionApplication> missionapplication;
        List<CI.Models.Skill> skill;
        List<CI.Models.MissionTheme> theme;
        List<CI.Models.Country> country;
        List<CI.Models.City> city;
        public AdminMission(CiPlatformContext db)
        {
            _db = db;
            missions = _db.Missions.ToList();
            missionapplication = _db.MissionApplications.ToList();
            skill = _db.Skills.ToList();
            theme= _db.MissionThemes.ToList();
            country = _db.Countries.ToList();
            city= _db.Cities.ToList();
         

        }


        //public AdminMission AddMission()
        //{

        //    User? user = _db.Users.FirstOrDefault(c => c.UserId == User_id);
        //    AdminMission adminMission = new AdminMission()
        //    {




        //        FirstName = user.FirstName,
        //        LastName = user.LastName,
        //        Email = user.Email,
        //        Availablity = user.Availablity,
        //        Title = user.Title,
        //        Manager = user.Manager,
        //        CountryId = user.CountryId,
        //        CityId = user.CityId,
        //        Department = user.Department,
        //        EmployeeId = user.EmployeeId,
        //        ProfileText = user.ProfileText,
        //        LinkedInUrl = user.LinkedInUrl,
        //        WhyIVolunteer = user.WhyIVolunteer,
        //        UserId = user.UserId,
        //    };


        //    if (country == 0)
        //    {

        //        List<Country> countries = _db.Countries.ToList();
        //        List<Skill> skills = _db.Skills.ToList();
        //        List<City> cities = _db.Cities.ToList();
        //        return new ProfileViewModel { Countries = countries, Cities = cities, Skills = skills, user = editUser };
        //    }
        //    else
        //    {
        //        List<City> cities = _db.Cities.Where(c => c.CountryId == country).ToList();
        //        return new ProfileViewModel { Cities = cities, user = editUser };
        //    }
        //}





        public List<Models.Country> GetAllCountry()
        {
            country = country.ToList();
            return country;
        }
        public List<Models.City> GetAllCities()
        {
            city=city.ToList();
            return city;
        }

        public List<Models.Mission> GetAllMission()
        {
            missions = missions.ToList();
            return missions;
        }

        public bool DeleteMission(Models.Mission mission)
        {
            _db.Missions.Update(mission);
            _db.SaveChanges();
            return true;
        }

        public Models.Mission GetMissionById(long id)
        {
            return _db.Missions.Where(x => x.MissionId == id).FirstOrDefault();

        }




        public List<Models.MissionApplication> GetAllMissionApplication()
        {
            //missionapplication = missionapplication.ToList();
            missionapplication = missionapplication.Where(x => x.ApprovalStatus == "PENDING").ToList();
            return missionapplication;
        }

        public MissionApplication GetMissionApplicationById(long id)
        {
            return _db.MissionApplications.Where(x => x.MissionApplicationId == id).FirstOrDefault();
        }

        public bool DeclineUser(MissionApplication missionapplication)
        {
            _db.MissionApplications.Update(missionapplication);
            _db.SaveChanges();
            return true;
        }


        //mission skill////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public List<Models.Skill> GetAllSkil()
        {
            skill = skill.ToList();
            return skill;
        }

        public Skill GetSkillById(long id)
        {
            return _db.Skills.Where(x => x.SkillId == id).FirstOrDefault();
        }

        public bool DeclineSkill(Skill skill)
        {
            _db.Skills.Update(skill);
            _db.SaveChanges();
            return true;
        }
        public bool AddSkill(Skill skill)
        {
            _db.Skills.Add(skill);
            _db.SaveChanges();
            return true;
        }


        //mission theme//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        public List<Models.MissionTheme> GetAllTheme()
        {
            theme = theme.ToList();
            return theme;
        }
        public MissionTheme GetThemeById(long id)
        {
            return _db.MissionThemes.Where(x => x.MissionThemeId == id).FirstOrDefault();
        }
        public bool DeclineTheme(MissionTheme theme)
        {
            _db.MissionThemes.Update(theme);
            _db.SaveChanges();
            return true;
        }

        public bool AddTheme(MissionTheme theme)
        {
            _db.MissionThemes.Add(theme);
            _db.SaveChanges();
            return true;
        }

    }
}
