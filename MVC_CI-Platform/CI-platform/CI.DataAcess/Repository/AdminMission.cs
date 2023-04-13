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
        public AdminMission(CiPlatformContext db)
        {
            _db = db;
            missions = _db.Missions.ToList();
            missionapplication = _db.MissionApplications.ToList();
            skill = _db.Skills.ToList();
            theme= _db.MissionThemes.ToList();  
        }


        public List<Models.Mission> GetAllMission()
        {
            missions = missions.ToList();
            return missions;
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
