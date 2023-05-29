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

        //List<Models.Mission> GetData(string searchTerm);
        List<Models.Mission> GetAllMission();
        Models.ViewModels.AdminMission GetAllMissions();

        bool DeleteMission(Models.Mission mission);
        Models.Mission GetMissionById(long id);
        List<Models.City> GetAllCities();
        void DeleteDocument(MissionDocument missionDocument);
  
        void DeleteMedia(MissionMedia missionmeida);
        Models.MissionMedia GetMediaById(long id);


        List<Models.MissionMedia> GetAllMedia();
        List<Models.MissionDocument> GetAllDocumet();
        List<Models.Country> GetAllCountry();
        Models.ViewModels.AdminMission GetCityById(long id);
        bool AddMission(Models.Mission mission);

        bool savemedia(Models.MissionMedia missionMedia);
        bool savedocumet(Models.MissionDocument missionDocument);

        bool AddDoc(Models.MissionDocument missionDocument);
        Models.MissionDocument GetDocumentById(long id);
        GoalMission getGoalMissionByMissionId(long missionId);
        void UpdateGoalMission(GoalMission goalMission);
        void UpdateMission(Models.Mission mission);

        ////mission application///////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        MissionApplication GetMissionApplicationById(long id);
        bool DeclineUser(MissionApplication missionapplication);
        IEnumerable<MissionApplication> SearchMissionApplication(string searchString);
        IEnumerable<MissionApplication> GetMissionApplication();


        ////skill//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        bool SkillExists(string skillname);
        List<Models.Skill> GetAllSkil();
        Skill GetSkillById(long id);
        bool DeclineSkill(Skill skill);
        bool AddSkill(Skill skill);
        void GetMissionSkills(long id);

        IEnumerable<Skill> GetSkill();
        IEnumerable<Skill> SearchSkill(string searchString);


        ///theme////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        bool ThemeExists(string title);
        List<Models.MissionTheme> GetAllTheme();
        MissionTheme GetThemeById(long id);
        bool DeclineTheme(MissionTheme theme);
        bool AddTheme(MissionTheme theme);
        void GetThemeByMissionId(long id);
        bool DeclineThemeInMission(Models.Mission mission);

        IEnumerable<MissionTheme> GetTheme();
        IEnumerable<MissionTheme> SearchTheme(string searchString);
    }
}
