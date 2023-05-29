using CI_Platform.Entities.Data;
using CI_Platform.Entities.Models;
using CI_Platform.Entities.ViewModels;
using CI_Platform.Repository.Interface;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CI_Platform.Repository.Repository
{
    public class PlatformRepository : IPlatformRepository 
    {

        private readonly CiPlatformContext _ciPlatformContext;
        public PlatformRepository(CiPlatformContext ciPlatformContext)
        {
            _ciPlatformContext = ciPlatformContext;
        }

        public List<Mission> GetMissions()
        {
            var missions = _ciPlatformContext.Missions.ToList();
            return missions;
        }

        #region Get All Missions For Volunteering Page
        public List<MissionCard> GetMissionData(int userid)
        {
            var query = _ciPlatformContext.Missions.Where(mission => mission.DeletedAt == null).AsQueryable();
            var timeSheet = _ciPlatformContext.Timesheets.Where(timeSheet => timeSheet.DeletedAt == null).AsQueryable();
            var goalMission = _ciPlatformContext.GoalMissions.Where(goal => goal.Mission.DeletedAt == null && goal.Mission.MissionType == 1).AsQueryable();


            var missionQuery = query.Select(mission => new MissionCard()
            {
                mission = mission,
                city = mission.City.Name,
                applied = mission.MissionApplications.Any(Mapp => Mapp.UserId == userid && Mapp.DeletedAt == null),
                favMission = mission.FavoriteMissions.Any(Mfav => Mfav.UserId == userid && Mfav.DeletedAt == null),
                missionMediums = mission.MissionMedia.Where(media => media.DeletedAt == null).ToList(),
                progressBar = mission.MissionType == 1 ? ((float?)mission.Timesheets.Select(timesheet => timesheet.Action).Sum() / ((goalMission.FirstOrDefault(goal => goal.MissionId == mission.MissionId) != null) ? goalMission.FirstOrDefault(goal => goal.MissionId == mission.MissionId).GoalValue : 1) * 100) : 0,
                achievedProgress = mission.Timesheets.Select(timesheet => timesheet.Action ?? 0).Sum(),
                goalObjective = mission.MissionType == 1 ? goalMission.FirstOrDefault(g => g.MissionId == mission.MissionId).GoalObjectiveText : String.Empty,
                totalGoal =  goalMission.FirstOrDefault(A => A.MissionId == mission.MissionId).GoalValue,
                rating = (float?)mission.MissionRatings.Average(rating => rating.Rating),
                Theme = mission.Theme.Title,
                seatsleft = mission.TotalMission - mission.MissionApplications.Count(Mapp => Mapp.DeletedAt == null),
                skills = mission.MissionSkills.Select(skill => skill.Skill.SkillName).ToList(),
                volunteercount = mission.MissionRatings.Count,
            });
            return missionQuery.ToList();

        }
        #endregion

        #region All Filter Values For Platform Page
        public IEnumerable<SelectListItem> GetAllCities()
        {
            IEnumerable<SelectListItem> AllCityList = _ciPlatformContext.Cities.Where(x => x.DeletedAt == null).Select(
                c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.CityId.ToString()
                });
            return AllCityList;
        }

        public IEnumerable<SelectListItem> GetAllCountries()
        {
            IEnumerable<SelectListItem> AllCountryList = _ciPlatformContext.Countries.Where(x => x.DeletedAt == null).Select(
                c => new SelectListItem
                { 
                    Text = c.Name,
                    Value = c.CountryId.ToString() 
                });
            return AllCountryList;
        }

        public IEnumerable<SelectListItem> GetAllSkills()
        {
            IEnumerable<SelectListItem> AllSkillList = _ciPlatformContext.Skills.Where(x => x.DeletedAt == null).Select( 
                s => new SelectListItem
            {
                Text = s.SkillName,
                Value = s.SkillId.ToString()
            });
            return AllSkillList;
        }

        public IEnumerable<SelectListItem> GetAllThemes()
        {
            IEnumerable<SelectListItem> AllThemeList = _ciPlatformContext.MissionThemes.Where(x => x.DeletedAt == null).Select
                (t => new SelectListItem
                {
                    Text = t.Title,
                    Value = t.MissionThemeId.ToString()
                });
            return AllThemeList;
        }
        #endregion

        #region Filter Method For Platform Page With Pagination
        public PageList<MissionCard> FilterOnMission(MissionFilter missionFilter, long userId)
        {
            var query = _ciPlatformContext.Missions.Where(mission => mission.DeletedAt == null).AsQueryable();
            var timeSheet = _ciPlatformContext.Timesheets.Where(timeSheet => timeSheet.DeletedAt == null).AsQueryable();
            var goalMission = _ciPlatformContext.GoalMissions.Where(goal => goal.Mission.DeletedAt == null && goal.Mission.MissionType == 1).AsQueryable();

            #region Filter
            if (missionFilter.CityIds.Any())
                query = query.Where(mission => missionFilter.CityIds.Contains(mission.CityId));

            if (missionFilter.CountryIds.Any())
                query = query.Where(mission => missionFilter.CountryIds.Contains(mission.CountryId));

            if (missionFilter.ThemeIds.Any())
                query = query.Where(mission => missionFilter.ThemeIds.Contains(mission.ThemeId));

            if (missionFilter.SkillIds.Any())
                query = query.Where(mission => mission.MissionSkills.Any(skill => missionFilter.SkillIds.Contains(skill.SkillId)));

            if (!string.IsNullOrWhiteSpace(missionFilter.Search))
                query = query.Where(mission => mission.Title.ToLower().Contains(missionFilter.Search.ToLower()));
            #endregion

            #region Missions
            var missionQuery = query.Select(mission => new MissionCard()
            {
                mission = mission,
                city = mission.City.Name,
                applied = mission.MissionApplications.Any(Mapp => Mapp.UserId == userId && Mapp.DeletedAt == null),
                favMission = mission.FavoriteMissions.Any(Mfav => Mfav.UserId == userId && Mfav.DeletedAt == null),
                missionMediums = mission.MissionMedia.Where(media => media.DeletedAt == null).ToList(),
                progressBar = mission.MissionType == 1 ? ((float?)mission.Timesheets.Select(timesheet => timesheet.Action).Sum() / ((goalMission.FirstOrDefault(goal => goal.MissionId == mission.MissionId) != null) ? goalMission.FirstOrDefault(goal => goal.MissionId == mission.MissionId).GoalValue : 1) * 100) : 0,
                achievedProgress = mission.Timesheets.Select(timesheet => timesheet.Action ?? 0).Sum(),
                goalObjective = mission.MissionType == 1 ? goalMission.FirstOrDefault(g => g.MissionId == mission.MissionId).GoalObjectiveText : String.Empty,
                totalGoal =  goalMission.FirstOrDefault(A => A.MissionId == mission.MissionId).GoalValue,
                rating = (float?)mission.MissionRatings.Average(rating => rating.Rating),
                Theme = mission.Theme.Title,
                seatsleft = mission.TotalMission - mission.MissionApplications.Count(Mapp => Mapp.DeletedAt == null),
                skills = mission.MissionSkills.Select(skill => skill.Skill.SkillName).ToList(),
                volunteercount = mission.MissionRatings.Count,
            });
            #endregion

            #region Sort
            switch (missionFilter.SortBy)
            {
                case "1":
                    missionQuery = missionFilter.SortOrder == "Desc"
                    ? missionQuery.OrderByDescending(query => query.mission.CreatedAt) : missionQuery.OrderBy(query => query.mission.CreatedAt);
                    break;
                case "2":
                    missionQuery = missionFilter.SortOrder == "Desc"
                    ? missionQuery.OrderBy(query => query.mission.CreatedAt) : missionQuery.OrderBy(query => query.mission.CreatedAt);
                    break;
                case "3":
                    missionQuery = missionFilter.SortOrder == "Desc"
                    ? missionQuery.OrderBy(query => query.seatsleft) : missionQuery.OrderBy(query => query.seatsleft);
                    break;
                case "4":
                    missionQuery = missionFilter.SortOrder == "Desc"
                    ? missionQuery.OrderByDescending(query => query.seatsleft) : missionQuery.OrderBy(query => query.seatsleft);
                    break;
                case "5":
                    missionQuery = missionFilter.SortOrder == "Desc"
                    ? missionQuery.OrderByDescending(query => query.favMission) : missionQuery.OrderBy(query => query.favMission);
                    break;
                case "6":
                    missionQuery = missionFilter.SortOrder == "Desc"
                    ? missionQuery.OrderBy(query => query.mission.Deadline) : missionQuery.OrderBy(query => query.mission.Deadline);
                    break;
                default:
                    missionQuery = missionFilter.SortOrder == "Desc"
                    ? missionQuery.OrderByDescending(query => query.mission.CreatedAt) : missionQuery.OrderBy(query => query.mission.CreatedAt);
                    break;
            }
            #endregion


            var totalcount = missionQuery.Count();
            if (missionQuery.Any())
            {
                var pages = Math.Ceiling((float)totalcount / missionFilter.PageSize);
                if (missionFilter.PageNumber > pages)
                    missionFilter.PageNumber = (int)pages;
                var records = missionQuery.Skip((missionFilter.PageNumber - 1) * missionFilter.PageSize).Take(missionFilter.PageSize).ToList();
                return new PageList<MissionCard>(records, totalcount);
            }
            return new PageList<MissionCard>(new List<MissionCard>(), totalcount);

        }
        #endregion

        #region Volunteering Page
        public VolunteerMissionPage GetVolunteerMission(int id, int userId)
        {
            VolunteerMissionPage volunteerMission = new VolunteerMissionPage(); 
            var mission = GetMissionData(userId);  // mission data to find related mission
            var userRating = _ciPlatformContext.MissionRatings.FirstOrDefault(x => x.UserId == userId && x.MissionId == id);
            
            #region Related Mission
            var appliedmission = mission.FirstOrDefault(x => x.mission.MissionId.Equals(id) && x.mission.DeletedAt == null); // fetching applied mission by their id
            appliedmission.favMission = _ciPlatformContext.FavoriteMissions.Any(x => x.MissionId.Equals(id) && x.UserId == userId && x.DeletedAt == null); // fetching favourite mission by their userid & mission id 
            var relatedMission = mission.Where(msn => (msn.mission.CityId.Equals(appliedmission.mission.CityId) || msn.mission.CountryId.Equals(appliedmission.mission.CountryId) || msn.mission.ThemeId.Equals(appliedmission.mission.ThemeId)) && msn.mission.MissionId != id).Take(3).ToList();
            #endregion

            #region Post comment 
            var Comments = _ciPlatformContext.Comments.Where(comments => comments.MissionId == id).Select(comment => new CommentsModel
            {
                username = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase((comment.User.FirstName + " " + comment.User.LastName).ToLower()) ?? string.Empty,
                avtar = string.IsNullOrEmpty(comment.User.Avatar) ? "user1.png" : comment.User.Avatar,
                comment = comment,
            }).ToList();
            #endregion

            #region Documents for mission
            var missionDocuments = _ciPlatformContext.MissionDocuments.Where(x => x.MissionId == id).ToList();
            #endregion

            #region Recent Volunteer
            List<MissionApplication> applications = _ciPlatformContext.MissionApplications.Include(x => x.User).Where(m => m.MissionId == id && m.DeletedAt==null).ToList();
            #endregion

            #region Return to controller
            volunteerMission.UserRating = userRating?.Rating ?? 0;
            volunteerMission.missionCard = appliedmission;
            volunteerMission.relatedmission = relatedMission;
            volunteerMission.comment = Comments;
            volunteerMission.missionDocuments = missionDocuments;
            volunteerMission.volunteerAvtar = applications;
            return volunteerMission;
            #endregion

        }
        #endregion

        #region Add & Remove Mission From Favourite by User
        public void AddToFavourite(int userid , int missionid)
        {
            var favmission = _ciPlatformContext.FavoriteMissions.FirstOrDefault(x => x.UserId == userid && x.MissionId == missionid);
            if (favmission == null)
            {
                FavoriteMission favoriteMission = new FavoriteMission();
                favoriteMission.UserId = userid;
                favoriteMission.MissionId = missionid;
                _ciPlatformContext.FavoriteMissions.Add(favoriteMission);
                _ciPlatformContext.SaveChanges();
            }
            else
            {
                favmission.DeletedAt = null;
                _ciPlatformContext.FavoriteMissions.Update(favmission);
                _ciPlatformContext.SaveChanges();
            }
        }

        public void RemoveFromFavourite(int userid, int missionid)
        {
            var favmission = _ciPlatformContext.FavoriteMissions.FirstOrDefault(x => x.UserId == userid && x.MissionId == missionid);
            favmission.DeletedAt = DateTime.Now;
            _ciPlatformContext.FavoriteMissions.Update(favmission);
            _ciPlatformContext.SaveChanges();

        }
        #endregion

        #region Add Comment by User
        public void Postcomment(int userid , int missionid , string text)
        {
            Comment comment = new Comment();
            comment.UserId = userid;    
            comment.MissionId = missionid;
            comment.CommentText = text;

            _ciPlatformContext.Comments.Add(comment);
            _ciPlatformContext.SaveChanges();
        }
        #endregion

        #region Re-Commend to Co-Workers by User
        public void SendMail(List<int> userid , int missionid , int inviteuser)
        {
            var mission = _ciPlatformContext.Missions.FirstOrDefault(x => x.MissionId==missionid).Title;
            foreach(var item in userid)
            {
                var user = _ciPlatformContext.Users.FirstOrDefault(x => x.UserId == item);               
                var loginuser = _ciPlatformContext.Users.FirstOrDefault(x => x.UserId == inviteuser);
                var username = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase((loginuser.FirstName + " " + loginuser.LastName).ToLower());
                
                #region Send Mail
                var mailBody = "<h1>" + username + " Has Recommended You to This Mission." + "</h1><br><h2>The Title Of Mission is : " + "<h2>'" + mission + "'</h2><a href='https://localhost:44356/Platform/Volunteer?id=" + missionid + "'>Click Here To Open Mission</a>";

                // create email message
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse("devloper.testing2022@gmail.com"));
                email.To.Add(MailboxAddress.Parse(user.Email));
                email.Subject = "Recommend Mission";
                email.Body = new TextPart(TextFormat.Html) { Text = mailBody };

                // send email
                using var smtp = new SmtpClient();
                smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                smtp.Authenticate("devloper.testing2022@gmail.com", "zryemtpwhipptczr");
                smtp.Send(email);
                smtp.Disconnect(true);
                #endregion Send Mail

                MissionInvite missionInvite = new MissionInvite();
                missionInvite.MissionId = missionid;
                missionInvite.ToUserId = item;
                missionInvite.FromUserId = inviteuser ;

                _ciPlatformContext.MissionInvites.Add(missionInvite);
                _ciPlatformContext.SaveChanges();
            }

        }
        #endregion

        #region Apply For Mission by User
        public void ApplyMission(int userid , int missionid)
        {
            var applymission = _ciPlatformContext.MissionApplications.FirstOrDefault(x => x.UserId == userid && x.MissionId == missionid );
            if (applymission == null)
            {
                MissionApplication missionApplication = new MissionApplication();
                missionApplication.UserId = userid;
                missionApplication.MissionId = missionid;
                missionApplication.AppliedAt = DateTime.Now;

                _ciPlatformContext.MissionApplications.Add(missionApplication);
                _ciPlatformContext.SaveChanges();
            }
            else
            {
                applymission.DeletedAt = null;
                applymission.AppliedAt = DateTime.Now;
                _ciPlatformContext.SaveChanges();
            }
        }

        public void UnapplyMission(int userid , int missionid)
        {
            var applymission = _ciPlatformContext.MissionApplications.FirstOrDefault(x => x.UserId == userid && x.MissionId == missionid && x.DeletedAt == null);

            applymission.DeletedAt = DateTime.Now;
            _ciPlatformContext.MissionApplications.Update(applymission);
            _ciPlatformContext.SaveChanges(true);
        }
        #endregion

        #region Get All UserDetails For Sending Mail
        public IEnumerable<SelectListItem> GetUserDetails(int userid)
        {
            IEnumerable<SelectListItem> AlluserList = _ciPlatformContext.Users.Where(x => x.DeletedAt == null && x.UserId != userid && x.Role != "Admin").Select(
                s => new SelectListItem
                {
                    Text = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase((s.FirstName + "  " + s.LastName).ToLower()),
                    Value = s.UserId.ToString()
                });
            return AlluserList;
        }
        #endregion

        #region Rating by User
        public void RatingByUser(int userid,int missionid,int rating)
        {
            var userRating = _ciPlatformContext.MissionRatings.FirstOrDefault(x => x.UserId == userid && x.MissionId ==missionid );
            if (userRating == null)
            {
                MissionRating missionRating = new MissionRating();
                missionRating.UserId = userid;
                missionRating.MissionId = missionid;
                missionRating.Rating = rating;
                _ciPlatformContext.MissionRatings.Add(missionRating);
                _ciPlatformContext.SaveChanges();
            }
            else
            {
                userRating.Rating = rating;
                userRating.UpdatedAt = DateTime.Now;
                _ciPlatformContext.MissionRatings.Update(userRating);
                _ciPlatformContext.SaveChanges();

            }
        }
        #endregion

        #region Get Cities by Country For City Filtering in Platform Page
        public List<SelectListItem> GetCitiesByCountries(List<long> countryId , List<long> cityId) 
        {

            var cities = _ciPlatformContext.Cities.Where(city => countryId.Contains(city.CountryId)).Select(
                    city => new SelectListItem
                    {
                        Text = city.Name,
                        Value = city.CityId.ToString(),
                        Selected = cityId.Contains(city.CityId)

                    }).ToList();
            return cities;
        }
        #endregion


    }
}
