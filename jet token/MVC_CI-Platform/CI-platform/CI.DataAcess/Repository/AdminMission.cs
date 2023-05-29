using CI.Models;
using CI.Repository.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
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
        List<CI.Models.MissionDocument> document;
        public AdminMission(CiPlatformContext db)
        {
            _db = db;

            missions = _db.Missions.ToList();
            missionapplication = _db.MissionApplications.ToList();
            skill = _db.Skills.ToList();
            theme = _db.MissionThemes.ToList();
            country = _db.Countries.ToList();
            city = _db.Cities.ToList();
            medias = _db.MissionMedia.ToList();
            document = _db.MissionDocuments.ToList();



        }
        public bool AddMission(Models.Mission mission)
        {

            if (mission is not null)
            {
                var users = _db.Users.Where(x => x.Status == "1" && x.DeletedAt == null).ToList();
                Notification notification = new Notification();
                notification.NotificationText = "New Mission - " + mission.Title;
                notification.NotificationType = 2;
                notification.MissionId = mission.MissionId;
                foreach (var user in users)
                {
                    var notificationsettingrecord = _db.NotificationSettings.FirstOrDefault(user => user.UserId == user.UserId);
                    if (mission.MissionSkills.Any(missionSkill => user.UserSkills.Any(userSkill => userSkill.SkillId == missionSkill.SkillId)))
                    {


                        if (notificationsettingrecord.NewMessage == 1)
                        {

                            UserNotification userNotification = new UserNotification();
                            userNotification.UserId = user.UserId;
                            userNotification.CreatedAt = DateTime.Now;
                            notification.UserNotifications.Add(userNotification);
                        }
                    }
                }
                _db.Notifications.Add(notification);
            }


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
        }
        public bool AddDoc(Models.MissionDocument missionDocument)
        {
            _db.MissionDocuments.Add(missionDocument);
            _db.SaveChanges();
            return true;
        }

        public Models.ViewModels.AdminMission GetCityById(long id)
        {
            List<City> records = _db.Cities.Where(c => c.CountryId == id).ToList();
            return new Models.ViewModels.AdminMission { Cities = records };

        }




        public List<Models.Country> GetAllCountry()
        {
            country = country.ToList();
            return country;
        }
        public List<Models.City> GetAllCities()
        {
            city = city.ToList();
            return city;
        }

        public List<Models.MissionMedia> GetAllMedia()
        {
            medias = medias.ToList();
            return medias;
        }
        public List<Models.MissionDocument> GetAllDocumet()
        {
            document = document.ToList();
            return document;
        }

        public List<Models.Mission> GetAllMission()
        {
            missions = missions.Where(x => x.Status is true).ToList();
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
        public Models.MissionDocument GetDocumentById(long id)
        {
            return _db.MissionDocuments.Where(x => x.MissionDocumentId == id).FirstOrDefault();

        }
        public void DeleteDocument(MissionDocument missionDocument)
        {
            _db.MissionDocuments.Remove(missionDocument);
            _db.SaveChanges();

        }
        public Models.MissionMedia GetMediaById(long id)
        {
            return _db.MissionMedia.Where(x => x.MissionMediaId == id).FirstOrDefault();

        }
        public void DeleteMedia(MissionMedia missionmeida)
        {
            _db.MissionMedia.Remove(missionmeida);
            _db.SaveChanges();

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
           .Where(u => u.Mission.Title.Contains(searchString) && u.ApprovalStatus == "PENDING")
           .ToList();
        }

        public MissionApplication GetMissionApplicationById(long id)
        {
            return _db.MissionApplications.Where(x => x.MissionApplicationId == id).FirstOrDefault();
        }

        public bool DeclineUser(MissionApplication missionapplication)
        {
            if (missionapplication.ApprovalStatus == "APPROVE")
            {

                var notificationsettingrecord = _db.NotificationSettings.FirstOrDefault(User => User.UserId == missionapplication.UserId);
                if (notificationsettingrecord.FromMail == 1)
                {
                    var emailtext = "Mission request has been approve for this mission - " + missionapplication.Mission.Title;
                    //   _commonRepository.SendEmail(applicationrecord.User.Email, emailtext);
                    MailMessage message = new MailMessage();
                    SmtpClient smtp = new SmtpClient();
                    message.From = new MailAddress("ciplatform123@gmail.com");
                    message.To.Add(new MailAddress(missionapplication.User.Email));
                    message.Subject = " CI-Platform Notification";
                    message.IsBodyHtml = true;
                    message.Body = emailtext;
                    smtp.Port = 587;
                    smtp.Host = "smtp.gmail.com";
                    smtp.EnableSsl = true;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential("ciplatform123@gmail.com", "rqfhiijvqaifdehx");
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.Send(message);
                }
                if (notificationsettingrecord.ApplicationApprove == 1)
                {
                    Notification notification = new Notification();
                    notification.NotificationText = "Mission request Approve - " + missionapplication.Mission.Title;
                    notification.NotificationType = 1;
                    notification.MissionId = missionapplication.MissionId;
                    UserNotification userNotification = new UserNotification();
                    userNotification.UserId = missionapplication.UserId;
                    userNotification.CreatedAt = DateTime.Now;
                    notification.UserNotifications.Add(userNotification);
                    _db.Notifications.Add(notification);
                }
            }
            if (missionapplication.ApprovalStatus == "DECLINE")
            {
                //var notificationsettingrecord = _db.NotificationSettings.FirstOrDefault(user => user.UserId == missionapplication.UserId);
                var notificationsettingrecord = _db.NotificationSettings.FirstOrDefault(User => User.UserId == missionapplication.UserId);
                if (notificationsettingrecord.FromMail == 1)
                {
                    var emailtext = "Mission request has been decline for this mission - " + missionapplication.Mission.Title;
                    MailMessage message = new MailMessage();
                    SmtpClient smtp = new SmtpClient();
                    message.From = new MailAddress("ciplatform123@gmail.com");
                    message.To.Add(new MailAddress(missionapplication.User.Email));
                    message.Subject = " CI-Platform Notification";
                    message.IsBodyHtml = true;
                    message.Body = emailtext;
                    smtp.Port = 587;
                    smtp.Host = "smtp.gmail.com";
                    smtp.EnableSsl = true;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential("ciplatform123@gmail.com", "rqfhiijvqaifdehx");
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.Send(message);
                }
                if (notificationsettingrecord.ApplicationApprove == 1)
                {
                    Notification notification = new Notification();
                    notification.NotificationText = "Mission request Decline - " + missionapplication.Mission.Title;
                    notification.NotificationType = 1;
                    //notification.MissionId = missionapplication.MissionId;
                    UserNotification userNotification = new UserNotification();
                    userNotification.UserId = missionapplication.UserId;
                    userNotification.CreatedAt = DateTime.Now;
                    notification.UserNotifications.Add(userNotification);
                    _db.Notifications.Add(notification);
                }
            }

            _db.MissionApplications.Update(missionapplication);
            if (missionapplication.ApprovalStatus == "DECLINE")
            {
                _db.MissionApplications.Remove(missionapplication);

            }
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
            var records = _db.Missions.Where(x => x.ThemeId == id).ToList();
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
