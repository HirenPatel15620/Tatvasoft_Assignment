using CI.Models;
using CI.Models.ViewModels;
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
    public class AdminStory : IAdminStory
    {
        private CiPlatformContext _db;
  

        List<Models.Story> story = new List<Models.Story>();
        List<Models.Banner> banner = new List<Models.Banner>();
        public AdminStory(CiPlatformContext db)
        {
            _db = db;
       
            getAllDetails();


        }
        public void getAllDetails()
        {
            story = _db.Stories.ToList();
            banner = _db.Banners.ToList();

        }

        public IEnumerable<Models.Story> GetStory()
        {
            return _db.Stories.Where(x => x.Status != "PUBLISHED" && x.Status != "DELETE" && x.Status != "DRAFT").ToList();
        }

        public IEnumerable<Models.Story> SearchStory(string searchString)
        {
            return _db.Stories
                .Where(u => u.Title.Contains(searchString) && u.Status != "PUBLISHED" && u.Status != "DELETE" && u.Status != "DRAFT")
                .ToList();
        }





        public Models.Story GetStoryById(long id)
        {
            return _db.Stories.Where(x => x.StoryId == id).FirstOrDefault();
        }

        public bool DeclineStory(Models.Story story)
        {
            var storyrecord = _db.Stories.Where(x => x.StoryId == story.StoryId).Include(x => x.Mission).Include(x => x.User).FirstOrDefault();
            if (storyrecord != null)
            {
                if (story.Status == "PUBLISHED")
                {
                    var notificationsettingrecord = _db.NotificationSettings.FirstOrDefault(user => user.UserId == storyrecord.UserId);
                    if (notificationsettingrecord.FromMail == 1)
                    {
                        var emailtext = "Volunteering request has been approve for this mission - " + storyrecord.Mission.Title;
                        //_allRepository.Mission.SendEmail(story.User.Email, emailtext);
                        MailMessage message = new MailMessage();
                        SmtpClient smtp = new SmtpClient();
                        message.From = new MailAddress("ciplatform123@gmail.com");
                        message.To.Add(new MailAddress(storyrecord.User.Email));
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
                    if (notificationsettingrecord.StoryApprove == 1)
                    {
                        Notification notification = new Notification();
                        notification.NotificationText = "Story Approve - " + storyrecord.Mission.Title;
                        notification.NotificationType = 1;
                        notification.StoryId=story.StoryId;
                        UserNotification userNotification = new UserNotification();
                        userNotification.UserId = storyrecord.UserId;
                        userNotification.CreatedAt = DateTime.Now;
                        notification.UserNotifications.Add(userNotification);
                        _db.Notifications.Add(notification);
                    }
                }

                if (story.Status == "DECLINED")
                {
                    var notificationsettingrecord = _db.NotificationSettings.FirstOrDefault(user => user.UserId == storyrecord.UserId);
                    if (notificationsettingrecord.FromMail == 1)
                    {
                        var emailtext = "Volunteering request has been decline for this mission - " + storyrecord.Mission.Title;
                        // _commonRepository.SendEmail(storyrecord.User.Email, emailtext);
                    }
                    if (notificationsettingrecord.StoryApprove == 1)
                    {
                        Notification notification = new Notification();
                        notification.NotificationText = "Story Decline - " + storyrecord.Mission.Title;
                        notification.NotificationType = 1;
                        notification.StoryId = story.StoryId;
                        UserNotification userNotification = new UserNotification();
                        userNotification.UserId = storyrecord.UserId;
                        userNotification.CreatedAt = DateTime.Now;
                        notification.UserNotifications.Add(userNotification);
                        _db.Notifications.Add(notification);
                    }
                }
            }
                _db.Stories.Update(story);
            _db.SaveChanges();
            return true;
        }

        public bool DeleteStory(Models.Story story)
        {
            _db.Stories.Update(story);
            _db.SaveChanges();
            return true;
        }






        //banners





        public Models.ViewModels.Banner GetAllBanners()
        {
            banner = banner.OrderBy(ci => ci.SortOrder).ToList();
            Models.Banner mybanner = new Models.Banner();

            return new Models.ViewModels.Banner { BannerList = banner, banner = mybanner };
        }

        public Models.Banner GetBannerById(long id)
        {
            return _db.Banners.Where(x => x.BannerId == id).FirstOrDefault();
        }


        public bool DeleteBanner(Models.Banner banner)
        {
            _db.Banners.Remove(banner);
            _db.SaveChanges();
            return true;
        }

        public bool AddBanner(Models.Banner banner)
        {
            _db.Banners.Add(banner);
            _db.SaveChanges();
            return true;
        }

        public bool editbanner(Models.Banner banner)
        {
            _db.Banners.Update(banner);
            _db.SaveChanges();
            return true;
        }
    }
}
