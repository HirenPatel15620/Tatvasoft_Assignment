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
        public bool savedocumet(Models.MissionDocument missionDocument)
        {
            _db.MissionDocuments.Add(missionDocument);
            _db.SaveChanges();
            return true;
        } public bool AddDoc(Models.MissionDocument missionDocument)
        {
            _db.MissionDocuments.Add(missionDocument);
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

        public List<Models.MissionMedia> GetAllMedia()
        {
            medias=medias.ToList();
            return medias;
        }

        public List<Models.Mission> GetAllMission()
        {
            missions = missions.Where(x=>x.Status is true).ToList();
            return missions;
        }

        public Models.ViewModels.AdminMission GetAllMissions()
        {
            missions = missions.Where(x => x.Status is true).ToList();
            Models.Mission mymission = new Models.Mission();

            return new Models.ViewModels.AdminMission { Missions = missions };
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


        public GoalMission getGoalMissionByMissionId(long missionId)
        {
            return _db.GoalMissions.Where(x => x.MissionId == missionId).FirstOrDefault();
        }
        public void UpdateGoalMission(GoalMission goalMission)
        {
            _db.GoalMissions.Update(goalMission);
            _db.SaveChanges();
        }

        public void UpdateMission(Models.Mission mission)
        {
            _db.Missions.Update(mission);
            _db.SaveChanges();
        }


        /// mission application//////////////////////////////////////////////////////////////////////////////////////////////////////////////////

     

        public IEnumerable<MissionApplication> GetMissionApplication()
        {
            return _db.MissionApplications.Where(x => x.ApprovalStatus == "PENDING" && x.Mission.Status == true).ToList();
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
        public bool SkillExists(string skillname)
        {
            return _db.Skills.Any(s => s.SkillName == skillname);
        }
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

        public void GetMissionSkills(long id)
        {
            var records = _db.MissionSkills.Where(x => x.SkillId == id).ToList();
            foreach (var record in records)
            {
                record.SkillId = null;
            }


            _db.SaveChanges();

        }


        public IEnumerable<Skill> SearchSkill(string searchString)
        {
            return _db.Skills
                .Where(u => u.SkillName.Contains(searchString))
                .ToList();
        }


        //mission theme//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public bool ThemeExists(string title)
        {
            return _db.MissionThemes.Any(t => t.Title == title);
        }

        public List<Models.MissionTheme> GetAllTheme()
        {
            theme = theme.ToList();
            return theme;
        }
        public void GetThemeByMissionId(long id)
        {
            var records= _db.Missions.Where(x => x.ThemeId == id).ToList();
            foreach (var record in records)
            {
                record.ThemeId = null;
            }

            
            _db.SaveChanges();
            
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
