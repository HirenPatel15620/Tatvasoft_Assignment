using CI_Platform.Entities.Models;
using CI_Platform.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CI_Platform.Repository.Interface
{
    public interface IPlatformRepository 
    {


        List<Mission> GetMissions();
        /* List<City> GetCities();
         List<MissionTheme> GetMissionThemes();
 */
        PageList<MissionCard> FilterOnMission(MissionFilter missionFilter, long userId);

        List<MissionCard> GetMissionData(int userid);
        IEnumerable<SelectListItem> GetAllCities();

        IEnumerable<SelectListItem> GetAllCountries();

        IEnumerable<SelectListItem> GetAllSkills();

        IEnumerable<SelectListItem> GetAllThemes();

        IEnumerable<SelectListItem> GetUserDetails(int userid);

        VolunteerMissionPage GetVolunteerMission(int id , int userId);

        void AddToFavourite(int userid, int missionid);

        void RemoveFromFavourite(int userid, int missionid);

        void Postcomment(int userid, int missionid, string text);

        void SendMail(List<int> userid, int missionid, int inviteuser);

        void ApplyMission(int userid, int missionid);

        void UnapplyMission(int userid, int missionid);

        void RatingByUser(int userid, int missionid, int rating);

        List<SelectListItem> GetCitiesByCountries(List<long> countryId, List<long> cityId);
    }
}
