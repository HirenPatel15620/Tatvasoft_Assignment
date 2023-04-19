using CI.Models;
using CI.Repository.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
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
        List<CI.Models.MissionMedia> medias;
        public AdminMission(CiPlatformContext db)
        {
            _db = db;
            missions = _db.Missions.ToList();
            missionapplication = _db.MissionApplications.ToList();
            skill = _db.Skills.ToList();
            theme= _db.MissionThemes.ToList();
            country = _db.Countries.ToList();
            city= _db.Cities.ToList();
            medias = _db.MissionMedia.ToList();



        }
        public bool AddMission(Models.Mission mission)
        {
            _db.Missions.Add(mission);
             _db.SaveChanges();
            //_db.SaveChanges();
            return true;
        }
        public bool savemedia(Models.MissionMedia missionMedia)
        {
            _db.MissionMedia.Add(missionMedia);
            _db.SaveChanges();
            return true;
        }






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



        //public IEnumerable<AdminMission> GetMission()
        //{
        //    return AdminMission.ToList();
        //}

        //public IEnumerable<AdminMission> SearchMission(string searchString)
        //{
        //    return AdminMission
        //        .Where(u => u.Title.Contains(searchString))
        //        .ToList();
        //}






        /// mission application//////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public List<Models.MissionApplication> GetAllMissionApplication()
        {
            //missionapplication = missionapplication.ToList();
            missionapplication = missionapplication.Where(x => x.ApprovalStatus == "PENDING").ToList();
            return missionapplication;
        }

        public IEnumerable<MissionApplication> GetMissionApplication()
        {
            return _db.MissionApplications.Where(x => x.ApprovalStatus == "PENDING").ToList();
        }

        public IEnumerable<MissionApplication> SearchMissionApplication(string searchString)
        {
            return _db.MissionApplications
           .Include(u => u.Mission)
           .Where(u => u.Mission.Title.Contains(searchString) && u.ApprovalStatus=="PENDING")
           .ToList();
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

        public IEnumerable<Skill> GetSkill()
        {
            return _db.Skills.ToList();
        }

        public IEnumerable<Skill> SearchSkill(string searchString)
        {
            return _db.Skills
                .Where(u => u.SkillName.Contains(searchString))
                .ToList();
        }


        //mission theme//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        public List<Models.MissionTheme> GetAllTheme()
        {
            theme = theme.ToList();
            return theme;
        }
        public Models.Mission GetThemeByMissionId(long id)
        {
            return _db.Missions.Where(x => x.ThemeId == id).FirstOrDefault();
        }

        public bool DeclineThemeInMission(Models.Mission mission)
        {
            _db.Missions.Update(mission);
            _db.SaveChanges();
            return true;
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
        public IEnumerable<MissionTheme> GetTheme()
        {
            return _db.MissionThemes.ToList();
        }

        public IEnumerable<MissionTheme> SearchTheme(string searchString)
        {
            return _db.MissionThemes
                .Where(u => u.Title.Contains(searchString))
                .ToList();
        }

    }
}
