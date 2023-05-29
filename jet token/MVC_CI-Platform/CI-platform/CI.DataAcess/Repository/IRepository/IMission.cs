using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace CI.Repository.Repository.IRepository
{
    public interface IMission:IRepository<CI.Models.Mission>
    {
        void Save();
        Models.ViewModels.Mission GetAllMission(long id);
       
        Models.ViewModels.Mission GetFilteredMissions(List<string> countries,List<string>cities, List<string> themes, List<string> skills,string sort_by,long user_id, string explore);
        Models.ViewModels.Mission GetSearchMissions(string key);
        CI.Models.ViewModels.Volunteer_Mission Mission(long id,long user_id);
        IEnumerable<Models.ViewModels.Comment_Viewmodel> comment(long user_id,long mission_id,string comment,int length);
        bool apply_for_mission(long user_id, long mission_id);
        bool add_to_favourite(long user_id, long mission_id);
        bool Rate_mission(long user_id, long mission_id,int rating);
        bool Recommend(long user_id, long mission_id,List<long> co_workers);
        CI.Models.ViewModels.Volunteer_Mission Next_Volunteers(int count, long user_id, long mission_id);


        //notification
        List<Models.UserNotification> GetAllNotification(int userid);
        List<Models.UserNotification> GetUnreadNotification(int userid);
        void UpdateNotificationStatusById(long usernotificationid);
        Models.NotificationSetting GetNotificationSettingsById(int userid);
        void UpdateNotificationSettingsByUser(Models.NotificationSetting notificationSettings);
        void DoAllSettingInactive(Models.NotificationSetting notificationSettings);
        void DeleteNotificationsByUser(int userid);
        List<Models.User> GetAllUsersWithoutInActive();
        List<CI.Models.UserNotification> GetOlderNotifications(int userid);
        List<CI.Models.UserNotification> GetNewerNotifications(int userid);
        DateTime UpdateLastseenValue(int userid);





    }
}
