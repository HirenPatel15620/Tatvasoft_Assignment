using CI_Platform.Entities.Data;
using CI_Platform.Entities.Models;
using CI_Platform.Entities.ViewModels;
using CI_Platform.Repository.Interface;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Http;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CI_Platform.Repository.Repository
{
    public class StoryRepository : IStoryRepository
    {

        private readonly CiPlatformContext _ciPlatformContext;
        public StoryRepository(CiPlatformContext ciPlatformContext)
        {
            _ciPlatformContext = ciPlatformContext;
        }

        #region Get All Stories For Story Listing Page
        public PageList<StoryListingPageModel> GetStory(string keyword , int PageNumber , int PageSize)
        {
            
            var result = from s in _ciPlatformContext.Stories
                         join u in _ciPlatformContext.Users on s.UserId equals u.UserId
                         join m in _ciPlatformContext.Missions on s.MissionId equals m.MissionId
                         join mt in _ciPlatformContext.MissionThemes on m.ThemeId equals mt.MissionThemeId 
                         where string.IsNullOrEmpty(keyword) || s.Title.ToLower().Contains(keyword.ToLower())
                         where s.Status == 2 && u.DeletedAt == null && m.DeletedAt == null && s.DeletedAt == null 
                         select new StoryListingPageModel
                         {
                             Username = u.FirstName + " " + u.LastName,
                             Avtar = u.Avatar,
                             story = s,
                             whyIvol = u.WhyIVolunteer,
                             ThemeName = mt.Title,
                             MissionId = s.MissionId,
                             storyMedium = _ciPlatformContext.StoryMedia.Where(x => x.StoryId == s.StoryId).ToList()
        };
            var totalcount = result.Count();
            var records = result.Skip((PageNumber - 1) * PageSize).Take(PageSize).ToList();
            return new PageList<StoryListingPageModel>(records, totalcount);
        }
        #endregion

        #region Perticular One Story Details For StoryDetail Page
        public StoryListingPageModel StoryDetails(int storyid , int userId)
        {
            TextInfo textinfo = new CultureInfo("en-US", false).TextInfo;
            var result = from s in _ciPlatformContext.Stories
                         join u in _ciPlatformContext.Users on s.UserId equals u.UserId
                         where s.StoryId == storyid && s.DeletedAt == null
                         select new StoryListingPageModel
                         {
                             story = s,
                             Username = textinfo.ToTitleCase((u.FirstName + " " + u.LastName).ToLower()),
                             Avtar = u.Avatar,
                             whyIvol = u.WhyIVolunteer,
                             storyMedium = _ciPlatformContext.StoryMedia.Where(sm => sm.StoryId == storyid).ToList(),
                         };
            var r = result.FirstOrDefault();
            if(r != null)
            {
                if (r.story.Status == 2)
                {
                    r.story.Views++;
                    _ciPlatformContext.Stories.Update(r.story);
                    _ciPlatformContext.SaveChanges();
                }
                return r;
            }     
            return new StoryListingPageModel();

        }
        #endregion

        #region Get All Missions For Add Story by User 
        public IEnumerable<SelectListItem> GetAllMission(int userId)
        {
            IEnumerable<SelectListItem> AllMissionList = (from MApp in _ciPlatformContext.MissionApplications 
                                                          join M in _ciPlatformContext.Missions on MApp.MissionId equals M.MissionId
                                                          where MApp.UserId == userId && M.DeletedAt == null
                                                          && MApp.ApprovalStatus == 2  
                                                          && MApp.DeletedAt == null 
                                                          select M).Select(
                c => new SelectListItem
                {
                Text = c.Title,
                Value = c.MissionId.ToString(),
                Selected = c.MissionType == 0,
                });

            return AllMissionList;
        }
        #endregion

        #region Story CRUD by User in ShareYourStory Page
        public bool AddYourStory(Story story)
        {
            var storieSaved = _ciPlatformContext.Stories.FirstOrDefault(x => x.UserId == story.UserId && x.MissionId == story.MissionId);
            var storyMedia = _ciPlatformContext.StoryMedia.Where(x => x.StoryId == storieSaved.StoryId).ToList();
            if (storieSaved.MissionId != 0 && storieSaved.Title != null && storieSaved.Description != null && storyMedia.Count != 0)
            {
                storieSaved.Status = 1;
                storieSaved.PublishedAt= DateTime.Now;
                _ciPlatformContext.Stories.Update(storieSaved);
                _ciPlatformContext.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        public void SaveYourStory(int userid, int missionid, string title, DateTime date, string description, List<IFormFile> fileList,List<string>? delImgList , List<string>? VideoUrl)
        {
            var alreadysaved = _ciPlatformContext.Stories.FirstOrDefault(x => x.UserId == userid && x.MissionId == missionid);
            if (alreadysaved == null)
            {

                Story saveStory = new Story();
                saveStory.UserId = userid;
                saveStory.MissionId = missionid;
                saveStory.Title = title;
                saveStory.Description = description;
                saveStory.Status = 0;   
                
                

            _ciPlatformContext.Stories.Add(saveStory);
            _ciPlatformContext.SaveChanges();

                if (VideoUrl != null)
                {
                    foreach (var file in VideoUrl)
                    {
                        StoryMedium storyMedium = new StoryMedium();
                        storyMedium.StoryId = alreadysaved.StoryId;
                        storyMedium.Path = file;
                        storyMedium.Type = "video";
                        _ciPlatformContext.StoryMedia.Add(storyMedium);
                        _ciPlatformContext.SaveChanges();
                    }
                }

                foreach (var file in fileList)
                {
                    string filename = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string uploadfilepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\media", filename);

                    using (var filestream = new FileStream(uploadfilepath, FileMode.Create))
                    {
                        file.CopyTo(filestream);
                    }
                    string uploadDBpath = filename;
                    var uploadImage = new StoryMedium()
                    {
                        StoryId = saveStory.StoryId,
                        Type = Path.GetExtension(file.FileName),
                        CreatedAt = DateTime.Now,
                        Path = uploadDBpath,
                    };
                    _ciPlatformContext.Add(uploadImage);

                }
                _ciPlatformContext.SaveChanges();
           
            }
            else
            {

                alreadysaved.Title = title;
                alreadysaved.Description = description;

                _ciPlatformContext.Stories.Update(alreadysaved);
                _ciPlatformContext.SaveChanges();

                if (fileList != null)
                {

                    foreach (var file in fileList)
                    {
                        string filename = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        string uploadfilepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\media", filename);

                        using (var filestream = new FileStream(uploadfilepath, FileMode.Create))
                        {
                            file.CopyTo(filestream);
                        }
                        string uploadDBpath = filename;
                        var uploadImage = new StoryMedium()
                        {
                            StoryId = alreadysaved.StoryId,
                            Type = Path.GetExtension(file.FileName),
                            CreatedAt = DateTime.Now,
                            Path = uploadDBpath,
                        };
                        _ciPlatformContext.Add(uploadImage);
                        _ciPlatformContext.SaveChanges();
                    }
                }
                if(delImgList != null)
                {
                    foreach (var file in delImgList)
                    {
                        string uploadfile = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\media", file);
                        var del = _ciPlatformContext.StoryMedia.FirstOrDefault(x => x.Path == file && x.StoryId == alreadysaved.StoryId);
                        _ciPlatformContext.StoryMedia.Remove(del);
                        _ciPlatformContext.SaveChanges();
                        if (File.Exists(uploadfile))
                        {
                            File.Delete(uploadfile);
                        }

                    }
                }
                var storyMedia = _ciPlatformContext.StoryMedia.Where(x => x.StoryId == alreadysaved.StoryId && x.Type == "video").AsQueryable();
                var removeURL = storyMedia.Where(x => !VideoUrl.Contains(x.Path));
                _ciPlatformContext.RemoveRange(removeURL);
                var addURL =  VideoUrl.Where(x => !storyMedia.Select(sm=>sm.Path).Contains(x));
                if (addURL.Any())
                {
                    foreach(var item in addURL)
                    {
                        var url = new StoryMedium { Path = item, StoryId = alreadysaved.StoryId, Type = "video" };
                        _ciPlatformContext.Add(url);
                    }
                }
                _ciPlatformContext.SaveChanges();

            }

        }
        #endregion

        #region Fetch Draft Story Details For User
        public SavedDraftModel SavedDraftStory(int userid, int missionid)
        {
            SavedDraftModel savedDraftModel = new SavedDraftModel();
            var story = _ciPlatformContext.Stories.FirstOrDefault(x => x.UserId == userid && x.MissionId == missionid);
            if(story != null)
            {

                savedDraftModel.DraftStory = story;
                var media = _ciPlatformContext.StoryMedia.Where(x => x.StoryId == story.StoryId && x.Type != "video").Select(x => x.Path).ToList();
                var video = _ciPlatformContext.StoryMedia.Where(x => x.StoryId == story.StoryId && x.Type == "video").Select(x=> x.Path).ToList();
                savedDraftModel.VideoURL = video;
                if(media != null)
                {
                    savedDraftModel.DraftMedia = media;
                }
                return savedDraftModel;
            }
            return null;

        }
        #endregion

        #region Get All TimeSheet Data For TimeSheet Page
        public VolunteeringTimesheetModel GetTimesheetData(int userid)
        {
            VolunteeringTimesheetModel volunteeringTimesheetModel = new VolunteeringTimesheetModel();

            var missionList = GetAllMission(userid).ToList();
            volunteeringTimesheetModel.MissionList = missionList;


            var timeSheet = from t in _ciPlatformContext.Timesheets
                            join m in _ciPlatformContext.Missions on t.MissionId equals m.MissionId
                            where t.UserId == userid
                            select new TimeSheetModel
                            {
                                MissionName = m.Title,
                                MissionType = m.MissionType,
                                missionStartDate = m.StartDate ?? DateTime.Now,
                                missionEndDate = m.EndDate > DateTime.Now ? DateTime.Now : m.EndDate ?? DateTime.Now,
                                timeSheet = t
                            };
            volunteeringTimesheetModel.timesheetModel = timeSheet.ToList();
            

            return volunteeringTimesheetModel;
        }
        #endregion

        #region TimeSheet CRUD
        public void EditTimesheet(Timesheet model,int userid)
        {
            var editTimeSheet = _ciPlatformContext.Timesheets.FirstOrDefault(t => t.UserId == userid && t.TimesheetId == model.TimesheetId);
            if(editTimeSheet != null)
            {
                
                editTimeSheet.DateVolunteered = model.DateVolunteered;
                editTimeSheet.Notes = model.Notes;
                editTimeSheet.Time = model.Time;
                editTimeSheet.Action = model.Action;

                _ciPlatformContext.Timesheets.Update(editTimeSheet);
                 _ciPlatformContext.SaveChanges();
            }
            else
            {
                Timesheet timesheet = new Timesheet();
                
                timesheet.UserId = userid;
                timesheet.MissionId = model.MissionId;
                timesheet.Notes = model.Notes;
                timesheet.Time = model.Time;
                timesheet.DateVolunteered = model.DateVolunteered;
                timesheet.Action = model.Action;

                _ciPlatformContext.Timesheets.Add(timesheet);
                _ciPlatformContext.SaveChanges();
            }
        }

        public void DeleteTimesheet(int userid , int TimesheetId)
        {
            var deleteTimeSheet = _ciPlatformContext.Timesheets.FirstOrDefault(t => t.UserId == userid && t.TimesheetId == TimesheetId);
            _ciPlatformContext.Timesheets.Remove(deleteTimeSheet);
            _ciPlatformContext.SaveChanges();
        }

        public TimeSheetModel SendMissionDateRange(int missionid)
        {
            var mission = _ciPlatformContext.Missions.FirstOrDefault(m => m.MissionId == missionid && m.DeletedAt == null);
            TimeSheetModel model = new TimeSheetModel();
            model.missionStartDate = mission.StartDate ?? DateTime.Now;
            model.missionEndDate = mission.EndDate > DateTime.Now ? DateTime.Now : mission.EndDate ?? DateTime.Now;
            return model;

        }
        #endregion

        #region Story Invite by User
        public void SendMailForStoryInvite(List<int> userid, int storyid, int inviteuser)
        {
            var story = _ciPlatformContext.Stories.FirstOrDefault(x => x.StoryId == storyid).Title;
            foreach (var item in userid)
            {
                var user = _ciPlatformContext.Users.FirstOrDefault(x => x.UserId == item);
                var loginuser = _ciPlatformContext.Users.FirstOrDefault(x => x.UserId == inviteuser);
                var username = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase((loginuser.FirstName + " " + loginuser.LastName).ToLower());

                #region Send Mail
                var mailBody = "<h1>" + username + " Has Recommended You to This Story." + "</h1><br><h2>The Title Of Story is : " + "<h2>'" + story + "'</h2><a href='https://localhost:44356/Story/StoryDetails?id=" + storyid + "'>Click Here To Open Story</a>";

                // create email message
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse("devloper.testing2022@gmail.com"));
                email.To.Add(MailboxAddress.Parse(user.Email));
                email.Subject = "Recommend Story";
                email.Body = new TextPart(TextFormat.Html) { Text = mailBody };

                // send email
                using var smtp = new SmtpClient();
                smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                smtp.Authenticate("devloper.testing2022@gmail.com", "zryemtpwhipptczr");
                smtp.Send(email);
                smtp.Disconnect(true);
                #endregion Send Mail

                StoryInvite storyInvite = new StoryInvite();
                storyInvite.StoryId = storyid;
                storyInvite.ToUserId = item;
                storyInvite.FromUserId = inviteuser;

                _ciPlatformContext.StoryInvites.Add(storyInvite);
                _ciPlatformContext.SaveChanges();
            }

        }
        #endregion

    }
}
