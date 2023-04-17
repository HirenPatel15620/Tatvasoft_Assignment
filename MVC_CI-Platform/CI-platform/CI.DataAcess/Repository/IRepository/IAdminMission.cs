using CI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI.Repository.Repository.IRepository
{
    public interface IAdminMission
    {

        List<Models.Mission> GetAllMission();

        bool DeleteMission(Models.Mission mission);
        Models.Mission GetMissionById(long id);
        List<Models.City> GetAllCities();
        List<Models.Country> GetAllCountry();





        List<Models.MissionApplication> GetAllMissionApplication();
        MissionApplication GetMissionApplicationById(long id);
        bool DeclineUser(MissionApplication missionapplication);
        List<Models.Skill> GetAllSkil();
        Skill GetSkillById(long id);
        bool DeclineSkill(Skill skill);
        bool AddSkill(Skill skill);
        List<Models.MissionTheme> GetAllTheme();
        MissionTheme GetThemeById(long id);
        bool DeclineTheme(MissionTheme theme);
        bool AddTheme(MissionTheme theme);
    }
}
