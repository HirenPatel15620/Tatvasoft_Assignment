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

        //mission//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //IEnumerable<AdminMission> SearchMission(string searchString);
        //IEnumerable<AdminMission> GetMission();


        List<Models.Mission> GetAllMission();

        bool DeleteMission(Models.Mission mission);
        Models.Mission GetMissionById(long id);
        List<Models.City> GetAllCities();
        List<Models.Country> GetAllCountry();

        bool AddMission(Models.Mission mission);

        bool savemedia(Models.MissionMedia missionMedia);



        ////mission application///////////////////////////////////////////////////////////////////////////////////////////////////////////////////

      
        MissionApplication GetMissionApplicationById(long id);
        bool DeclineUser(MissionApplication missionapplication);
        IEnumerable<MissionApplication> SearchMissionApplication(string searchString);
        IEnumerable<MissionApplication> GetMissionApplication();


        ////skill//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        List<Models.Skill> GetAllSkil();
        Skill GetSkillById(long id);
        bool DeclineSkill(Skill skill);
        bool AddSkill(Skill skill);

        IEnumerable<Skill> GetSkill();
        IEnumerable<Skill> SearchSkill(string searchString);


        ///theme////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        List<Models.MissionTheme> GetAllTheme();
        MissionTheme GetThemeById(long id);
        bool DeclineTheme(MissionTheme theme);
        bool AddTheme(MissionTheme theme);
        Models.Mission GetThemeByMissionId(long id);
        bool DeclineThemeInMission(Models.Mission mission);

        IEnumerable<MissionTheme> GetTheme();
        IEnumerable<MissionTheme> SearchTheme(string searchString);
    }
}
