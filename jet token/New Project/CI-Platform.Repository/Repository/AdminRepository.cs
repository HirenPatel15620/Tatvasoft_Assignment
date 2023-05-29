using CI_Platform.Entities.Data;
using CI_Platform.Entities.Models;
using CI_Platform.Entities.ViewModels;
using CI_Platform.Repository.Interface;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Web.Mvc;

namespace CI_Platform.Repository.Repository
{
    public class AdminRepository : IAdminRepository
    {
        private readonly CiPlatformContext _ciPlatformContext;
        public AdminRepository(CiPlatformContext ciPlatformContext)
        {
            _ciPlatformContext = ciPlatformContext;
        }

        #region Get Users For AdminPanel
        public List<User> GetUserDataForAdmin(string search)
        {
            var user = _ciPlatformContext.Users.Where(user => (string.IsNullOrEmpty(search) || user.FirstName != null && user.FirstName.ToLower().Contains(search.ToLower()) || user.LastName != null && user.LastName.ToLower().Contains(search.ToLower())) && user.DeletedAt == null && user.Role != "Admin");
            return user.ToList();

        }
        #endregion

        #region Get CMS For AdminPanel
        public List<CmsPage> GetCMSPageForAdmin(string search)
        {
            var cms = _ciPlatformContext.CmsPages.Where(cms => (string.IsNullOrEmpty(search) || cms.Title.ToLower().Contains(search.ToLower())) && cms.DeletedAt == null);
            return cms.ToList();

        }
        #endregion

        #region Get Stories For AdminPanel
        public List<StoryAdminModel> GetStoryDataForAdmin(string search)
       {
            TextInfo textinfo = new CultureInfo("en-US", false).TextInfo;
            var story = _ciPlatformContext.Stories.Where(story => story.Mission.DeletedAt == null && story.User.DeletedAt == null && (string.IsNullOrEmpty(search) || story.Title != null && story.Title.ToLower().Contains(search.ToLower()) || story.Mission.Title.ToLower().Contains(search.ToLower())) && story.DeletedAt == null && story.Status != 0).OrderBy(s => s.Title)
                .Select(story => new StoryAdminModel
                {
                    story = story,
                    Username = textinfo.ToTitleCase((story.User.FirstName + "  " + story.User.LastName).ToLower()),
                    MissionTitle = story.Mission.Title,
                });
            return story.ToList();
        }
        #endregion

        #region Get Missions For AdminPanel
        public List<Mission> GetMissionDataForAdmin(string search) 
        {
            var mission = _ciPlatformContext.Missions.Where(m => (string.IsNullOrWhiteSpace(search) || m.Title.ToLower().Contains(search.ToLower())) && m.DeletedAt == null).OrderBy(m => m.Title);
            return mission.ToList();
        }
        #endregion

        #region Get Mission Themes For AdminPanel
        public List<MissionTheme> GetMissionThemeForAdmin(string search)
        {
            var themes = _ciPlatformContext.MissionThemes.Where(theme =>  (string.IsNullOrWhiteSpace(search) || theme.Title.ToLower().Contains(search.ToLower())) && theme.DeletedAt == null ).OrderBy(t => t.Title);
            return themes.ToList();
        }
        #endregion

        #region Get Skills For AdminPanel
        public List<Skill> GetSkillForAdmin(string search)
        {
            var skillls = _ciPlatformContext.Skills.Where(skill =>   (string.IsNullOrWhiteSpace(search) || skill.SkillName.ToLower().Contains(search.ToLower())) && skill.DeletedAt == null).OrderBy(skill => skill.SkillName);

            return skillls.ToList();
        }
        #endregion

        #region Get Banners For AdminPanel
        public List<Banner> GetBannerDataForAdmin(string search)
        {
            var banner = _ciPlatformContext.Banners.Where(bner =>  (string.IsNullOrEmpty(search) || bner.Text != null && bner.Text.ToLower().Contains(search.ToLower()) || bner.SortOrder.ToString().Contains(search))  && bner.DeletedAt == null);
            return banner.ToList();
        }
        #endregion

        #region Get Mission Applications For AdminPanel
        public List<MissionApplicationAdminModel> GetMissionApplicationForAdmin(string search)
        {
            TextInfo textinfo = new CultureInfo("en-US", false).TextInfo;
            var missionApplications = _ciPlatformContext.MissionApplications.Where(m => m.Mission.DeletedAt == null && m.User.DeletedAt == null && (string.IsNullOrWhiteSpace(search) || m.Mission.Title.ToLower().Contains(search.ToLower()) || m.MissionId.ToString().Contains(search.ToLower()) || m.UserId.ToString().Contains(search.ToLower())) && m.ApprovalStatus == 1 && m.DeletedAt == null )
                .Select(mApp => new MissionApplicationAdminModel
                {
                    missionApplication = mApp,
                    MissionTitle = mApp.Mission.Title,
                    UserName =  textinfo.ToTitleCase((mApp.User.FirstName + "  " + mApp.User.LastName).ToLower()),
                });
            return missionApplications.ToList();
        }
        #endregion


        /*  -----------------------------------    User tab     ---------------------------------------------- */

        #region User CRUD
        public List<SelectListItem> GetCountryListForAddUserbyAdmin()
        {
            var country = _ciPlatformContext.Countries.AsQueryable().OrderBy(c => c.Name);
            var result = country.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.CountryId.ToString(),
            }).ToList();
            return result;
        }

        public User FetchUserForEdit(long userid)
        {
            var existUser = _ciPlatformContext.Users.FirstOrDefault(u => u.UserId == userid);
            if (existUser == null)
            {
                return new User();
            }
            return existUser;

        }

        public bool UserCRUDbyAdmin(UserDetailsModel model)
        {
            
           
                if (model.UserId != 0)
                {
                    var user = _ciPlatformContext.Users.FirstOrDefault(u => u.UserId == model.UserId);
                    if(user != null)
                    {
                        string avtarName;
                        if (model.Avatar != null)
                        {
                            if (user.Avatar != null)
                            {
                                string delImagwe = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\media", user.Avatar);
                                if (File.Exists(delImagwe))
                                {
                                    File.Delete(delImagwe);
                                }
                            }
                            avtarName = Guid.NewGuid().ToString() + Path.GetExtension(model.Avatar.FileName);
                            string uploadAvtarPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\media", avtarName);

                            using (var filestream = new FileStream(uploadAvtarPath, FileMode.Create))
                            {
                                model.Avatar.CopyTo(filestream);
                            }
                            user.Avatar = avtarName;
                        }
                        
                
                        user.FirstName = model.FirstName;
                        user.LastName = model.LastName;
                        user.EmployeeId = model.EmployeeId;
                        user.Department = model.Department;
                        user.CityId = model.CityId;
                        user.CountryId = model.CountryId;
                        user.Status = model.Status;
                        user.ProfileText = model.ProfileText;
                        


                        _ciPlatformContext.Users.Update(user);
                        _ciPlatformContext.SaveChanges();
                    }

                    return true;
                }
                else
                {
                    var alreadyExistUser = _ciPlatformContext.Users.Any(user => user.Email == model.Email);
                    if (alreadyExistUser == true)
                    {
                        return false;
                    }
                    else
                    { 
                        string avtarName;
                        if (model.Avatar != null)
                        {
                            avtarName = Guid.NewGuid().ToString() + Path.GetExtension(model.Avatar.FileName);
                            string uploadAvtarPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\media", avtarName);

                            using (var filestream = new FileStream(uploadAvtarPath, FileMode.Create))
                            {
                                model.Avatar.CopyTo(filestream);
                            }

                        }
                        else { avtarName = ""; }
                        var user = new User()
                        {
                            FirstName = model.FirstName,
                            LastName = model.LastName,
                            Email = model.Email,
                            Password = Crypto.HashPassword(model.Password),
                            EmployeeId = model.EmployeeId,
                            Department = model.Department,
                            CityId = model.CityId,
                            CountryId = model.CountryId,
                            Status = model.Status,
                            ProfileText = model.ProfileText,
                            Avatar = avtarName,
                            Role = "User",
                        };

                        _ciPlatformContext.Users.Add(user);
                        _ciPlatformContext.SaveChanges();

                        return true;
                    }
                }

            
        }

        public void DeleteUserbyAdmin(long userid)
        {
            var user = _ciPlatformContext.Users.FirstOrDefault(u => u.UserId == userid);
            if (user != null)
            {
                user.DeletedAt = DateTime.Now;
                _ciPlatformContext.Users.Update(user);
                _ciPlatformContext.SaveChanges();
            }
        }
        #endregion

        /*  -----------------------------------    CMS tab     ---------------------------------------------- */

        #region CMS CRUD
        public void CmsCRUDbyAdmin(CmsPage model)
        {
            if (model.CmsPageId != 0)
            {

                var cms = _ciPlatformContext.CmsPages.FirstOrDefault(u => u.CmsPageId == model.CmsPageId);
                if(cms != null)
                {
                    cms.Title = model.Title;
                    cms.Description = model.Description;    
                    cms.Slug = model.Slug;
                    cms.Status = model.Status;

                    _ciPlatformContext.CmsPages.Update(cms);
                    _ciPlatformContext.SaveChanges();
                }
            }
            else
            {
               
                var cms = new CmsPage()
                {
                    Title = model.Title,
                    Description = model.Description,
                    Slug = model.Slug,
                    Status = model.Status,
                };

                _ciPlatformContext.CmsPages.Add(cms);
                _ciPlatformContext.SaveChanges();

            }
        }

        public void CMSDeletebyAdmin(long cmsid)
        {
            var cms = _ciPlatformContext.CmsPages.FirstOrDefault(u => u.CmsPageId == cmsid);
            if(cms != null)
            {
                cms.DeletedAt = DateTime.Now;
                _ciPlatformContext.CmsPages.Update(cms);
                _ciPlatformContext.SaveChanges();
            }
        }

        public CmsPage FetchCMSForAdmin(long cmsId)
        {
            var cms = _ciPlatformContext.CmsPages.FirstOrDefault(cms => cms.CmsPageId == cmsId);
            if(cms == null)
            {
                return new CmsPage();
            }
            return cms;
        }
        #endregion

        /*  -----------------------------------    Story tab     ---------------------------------------------- */

        #region Story CRUD
        public void StoryApprovedbyAdmin(long storyid)
        {
            var story = _ciPlatformContext.Stories.FirstOrDefault(s => s.StoryId == storyid);
            if (story != null)
            {
                story.Status = 2;
                _ciPlatformContext.Stories.Update(story);
                _ciPlatformContext.SaveChanges();
            }
        }

        public void StoryRejectedbyAdmin(long storyid)
        {
            var story = _ciPlatformContext.Stories.FirstOrDefault(s => s.StoryId == storyid);
            if( story != null)
            {
                story.Status = 3;
                _ciPlatformContext.Stories.Update(story);
                _ciPlatformContext.SaveChanges();
            }
        }

        public void StoryDeletedbyAdmin(long storyid)
        {
            var story = _ciPlatformContext.Stories.FirstOrDefault(s => s.StoryId == storyid);
            if (story != null)
            {
                story.DeletedAt = DateTime.Now;
                _ciPlatformContext.Stories.Update(story);
                _ciPlatformContext.SaveChanges();
            }
            
        }

        public StoryAdminModel GetStoryDetailsbyAdmin(long storyid)
        {
            
            TextInfo textinfo = new CultureInfo("en-US", false).TextInfo;
            var storyDetails = from s in _ciPlatformContext.Stories
                           join u in _ciPlatformContext.Users on s.UserId equals u.UserId
                           where s.StoryId == storyid
                           select new StoryAdminModel
                           {
                               story = s,
                               storyMedium = s.StoryMedia.ToList(),
                               Username = textinfo.ToTitleCase((u.FirstName + " " + u.LastName).ToLower()),
                               MissionTitle = s.Mission.Title,
                           };

            return storyDetails.FirstOrDefault() ?? new StoryAdminModel();
        }
        #endregion

        /*  -----------------------------------    Mission application tab     --------------------------------- */

        #region Mission Application CRUD
        public void MissionApplicationApprovedbyAdmin(long missionid)
        {
            var mission = _ciPlatformContext.MissionApplications.FirstOrDefault(m => m.MissionApplicationId == missionid);
            if (mission != null)
            {
                mission.ApprovalStatus = 2;
                _ciPlatformContext.MissionApplications.Update(mission);
                _ciPlatformContext.SaveChanges();
            }
        }

        public void MissionApplicationRejectbyAdmin(long missionid)
        {
            var mission = _ciPlatformContext.MissionApplications.FirstOrDefault(m => m.MissionApplicationId == missionid);
            if (mission != null)
            {
                mission.ApprovalStatus = 0;
                _ciPlatformContext.MissionApplications.Update(mission);
                _ciPlatformContext.SaveChanges();
            }
        }
        #endregion

        /*  -----------------------------------    Mission theme tab     ---------------------------------------- */

        #region Mission Theme CRUD
        public void ThemeCRUDbyAdmin(MissionTheme model)
        {
            if (model.MissionThemeId != 0)
            {

                var theme = _ciPlatformContext.MissionThemes.FirstOrDefault(t => t.MissionThemeId == model.MissionThemeId);
                if( theme != null)
                {
                    theme.Title = model.Title;
                    if(model.Status == 0)
                        theme.Status = 0;
                    else
                        theme.Status = 1;

                    _ciPlatformContext.MissionThemes.Update(theme);
                    _ciPlatformContext.SaveChanges();
                }
            }
            else
            {
                
                var theme = new MissionTheme()
                {
                    Title = model.Title,
                    Status = model.Status,

                };

                _ciPlatformContext.MissionThemes.Add(theme);
                _ciPlatformContext.SaveChanges();

            }
        }

        public MissionTheme FetchMissionThemeForAdmin(long themeid)
        {
            var theme = _ciPlatformContext.MissionThemes.FirstOrDefault(t => t.MissionThemeId.Equals(themeid));
            if (theme == null)
            {
                return new MissionTheme();
            }
            return theme;
        }

        public void DeleteMissionThemebyAdmin(long themeid)
        {
            var theme = _ciPlatformContext.MissionThemes.FirstOrDefault(t => t.MissionThemeId == themeid);
            if (theme != null)
            {
                theme.DeletedAt = DateTime.Now;
                _ciPlatformContext.MissionThemes.Update(theme);
                _ciPlatformContext.SaveChanges();
            }
        }
        #endregion

        /*  -----------------------------------    Mission skill tab     ---------------------------------------- */

        #region Mission Skill CRUD
        public void SkillCRUDbyAdmin(Skill model)
        {
            if (model.SkillId != 0)
            {

                var skill = _ciPlatformContext.Skills.FirstOrDefault(t => t.SkillId == model.SkillId);
                if (skill != null)
                {
                    skill.SkillName = model.SkillName;
                    if (model.Status == 0)
                        skill.Status = 0;
                    else
                        skill.Status = 1;

                    _ciPlatformContext.Skills.Update(skill);
                    _ciPlatformContext.SaveChanges();
                }
            }
            else
            {
                var skill = new Skill()
                {
                    SkillName = model.SkillName,
                    Status = model.Status,

                };

                _ciPlatformContext.Skills.Add(skill);
                _ciPlatformContext.SaveChanges();

            }
        }

        public Skill FetchMissionSkillForAdmin(long skillid)
        {
            var skill = _ciPlatformContext.Skills.FirstOrDefault(s => s.SkillId.Equals(skillid));
            if (skill == null)
            {
                return new Skill();
            }
            return skill;
        }

        public void DeleteMissionSkillbyAdmin(long skillid)
        {
            var skill = _ciPlatformContext.Skills.FirstOrDefault(s => s.SkillId == skillid);
            if (skill != null)
            {
                skill.DeletedAt = DateTime.Now;
                _ciPlatformContext.Skills.Update(skill);
                _ciPlatformContext.SaveChanges();
            }
        }
        #endregion

        /*  -----------------------------------    Mission  tab     ---------------------------------------------- */

        #region Mission CRUD
        public List<SelectListItem> GetAllSkillListForMission()
        {
            List<SelectListItem> AllSkillList = (from s in _ciPlatformContext.Skills 
                                                 orderby s.SkillName
                                                 where s.DeletedAt == null
                                                 select s).Select(
                skill => new SelectListItem
                {
                    Text = skill.SkillName,
                    Value = skill.SkillId.ToString(),
                }).ToList();

            return AllSkillList;
        }

        public List<SelectListItem> GetAllThemeListForMission()
        {
            List<SelectListItem> AllThemeList = (from t in _ciPlatformContext.MissionThemes
                                                 orderby t.Title
                                                 where t.DeletedAt == null
                                                 select t).Select(
                theme => new SelectListItem
                {
                    Text = theme.Title,
                    Value = theme.MissionThemeId.ToString(),
                }).ToList();

            return AllThemeList;
        }

        public List<SelectListItem> GetAllCountryListForMission()
        {
            List<SelectListItem> AllCountryList = (from c in _ciPlatformContext.Countries
                                                   orderby c.Name
                                                   where c.DeletedAt == null
                                                 select c).Select(
                country => new SelectListItem
                {
                    Text = country.Name,
                    Value = country.CountryId.ToString(),
                }).ToList();

            return AllCountryList;
        }

        public void MissionCRUDbyAdmin(MissionAdminModel model)
        {
            if(model.missionId == 0)
            {
                Mission mission = new Mission();

                mission.Title = model.missionName;
                mission.MissionType = model.missionType;
                mission.CityId = model.missionCityId;
                mission.CountryId = model.missionCountryId;
                mission.ThemeId = model.missionThemeId;
                mission.OrganizationName = model.organizationName;
                mission.OrganizationDetail = model.organizationDetails;
                mission.ShortDescription = model.shortDescription;
                mission.Description = model.description;
                mission.StartDate = model.startDate;
                mission.EndDate = model.endDate;
                mission.Deadline = model.deadline;
                mission.TotalMission = model.seats;

                _ciPlatformContext.Missions.Add(mission);
                _ciPlatformContext.SaveChanges();
                if (model.missionType == 1)
                {
                    var goalEntery = new GoalMission
                    {
                        GoalObjectiveText = model.Msn_Goal_Obj,
                        MissionId = mission.MissionId,
                        GoalValue = model.Msn_Goal_Action
                    };
                    _ciPlatformContext.GoalMissions.Add(goalEntery);
                    _ciPlatformContext.SaveChanges();
                }

                foreach (var item in model.skillIds)
                {
                    var missionSkill = new MissionSkill()
                    {
                        SkillId = item,
                        MissionId = mission.MissionId,
                    };
                    _ciPlatformContext.Add(missionSkill);
                }
                _ciPlatformContext.SaveChanges();

                if (model.VideoUrl != null)
                {
                    foreach (var file in model.VideoUrl)
                    {
                        var missionMedia = new MissionMedium()
                        {
                            MissionId = mission.MissionId,
                            MediaPath = file,
                            MediaType = "video",
                            MediaName = "videos",
                        };
                        _ciPlatformContext.Add(missionMedia);
                    }
                       _ciPlatformContext.SaveChanges();
                }

                if(model.fileList != null)
                {
                    foreach (var file in model.fileList)
                    {
                        string filename = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        string uploadfilepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\media", filename);

                        using (var filestream = new FileStream(uploadfilepath, FileMode.Create))
                        {
                            file.CopyTo(filestream);
                        }
                        string uploadDBpath = filename;
                        var uploadImage = new MissionMedium()
                        {
                            MissionId = mission.MissionId,
                            MediaType = Path.GetExtension(file.FileName),
                            CreatedAt = DateTime.Now,
                            MediaPath = uploadDBpath,
                            MediaName = "photos",
                        };
                        _ciPlatformContext.Add(uploadImage);

                    }
                    _ciPlatformContext.SaveChanges();
                }


                if(model.docList != null)
                {
                    foreach (var file in model.docList)
                    {
                        string filename = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        string uploadfilepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\media", filename);

                        using (var filestream = new FileStream(uploadfilepath, FileMode.Create))
                        {
                            file.CopyTo(filestream);
                        }
                        string uploadDBpath = filename;
                        var uploadDoc = new MissionDocument()
                        {
                            MissionId = mission.MissionId,
                            DocumentType = Path.GetExtension(file.FileName),
                            CreatedAt = DateTime.Now,
                            DocumentPath = uploadDBpath,
                            DocumentName = file.FileName,
                        };
                        _ciPlatformContext.Add(uploadDoc);

                    }
                    _ciPlatformContext.SaveChanges();
                }

            }
            else
            {
                var mission = _ciPlatformContext.Missions.FirstOrDefault(msn => msn.MissionId == model.missionId);
                if (mission != null)
                {
                    mission.ThemeId = model.missionThemeId;
                    mission.CityId = model.missionCityId;
                    mission.CountryId = model.missionCountryId;
                    mission.Title = model.missionName;
                    mission.ShortDescription = model.shortDescription;
                    mission.Description = model.description;
                    mission.StartDate = model.startDate;
                    mission.EndDate = model.endDate;
                    mission.Deadline = model.deadline;
                    mission.OrganizationDetail = model.organizationDetails;
                    mission.OrganizationName = model.organizationName;
                    mission.TotalMission = model.seats;
                    mission.UpdatedAt = DateTime.Now;

                    _ciPlatformContext.Missions.Update(mission);
                    _ciPlatformContext.SaveChanges();

                    if (model.missionType == 1)
                    {
                        var goalMission = _ciPlatformContext.GoalMissions.FirstOrDefault(goal => goal.MissionId == model.missionId);
                        if (goalMission != null)
                        {
                            goalMission.GoalObjectiveText = model.Msn_Goal_Obj;
                            goalMission.GoalValue = model.Msn_Goal_Action;
                            _ciPlatformContext.GoalMissions.Update(goalMission);
                            _ciPlatformContext.SaveChanges();
                        }
                    }

                    

                    if (model.skillIds != null)
                    {
                        var skills = _ciPlatformContext.MissionSkills.Where(x => x.MissionId == model.missionId);
                        var addSkills = model.skillIds.Except(skills.Select(x => x.SkillId));

                        var delSkills = skills.Where(msn => !model.skillIds.Contains(msn.SkillId));
                        _ciPlatformContext.MissionSkills.RemoveRange(delSkills);
                        _ciPlatformContext.SaveChanges();

                        foreach (var skill in addSkills)
                        {
                            var missionSkill = new MissionSkill
                            {
                                MissionId = model.missionId,
                                SkillId = skill,

                            };
                            _ciPlatformContext.MissionSkills.Add(missionSkill);
                            _ciPlatformContext.SaveChanges();
                        }
                        _ciPlatformContext.SaveChanges();
                       

                    }
                    if (model.fileList.Count > 0)
                    {
                        foreach (var file in model.fileList)
                        {
                            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                            string uploadfile = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\media", fileName);

                            using (var filestream = new FileStream(uploadfile, FileMode.Create))
                            {
                                file.CopyTo(filestream);
                            }

                            MissionMedium missionMedium = new MissionMedium();
                            missionMedium.MissionId = model.missionId;
                            missionMedium.MediaType = Path.GetExtension(file.FileName);
                            missionMedium.MediaPath = fileName;
                            missionMedium.MediaName = "photos";
                            _ciPlatformContext.MissionMedia.Add(missionMedium);
                            _ciPlatformContext.SaveChanges();

                        }
                    }

                    if (model.delImgList.Count > 0)
                    {
                        foreach (var delImg in model.delImgList)
                        {
                            string uploadfile = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\media", delImg);
                            var del = _ciPlatformContext.MissionMedia.FirstOrDefault(x => x.MediaPath == delImg && x.MissionId == model.missionId);
                            if(del != null)
                            {
                                _ciPlatformContext.MissionMedia.Remove(del);
                                _ciPlatformContext.SaveChanges();
                            }
                            if (File.Exists(uploadfile))
                            {
                                File.Delete(uploadfile);
                            }
                        }

                    }

                    if (model.VideoUrl != null)
                    {
                        var video = _ciPlatformContext.MissionMedia.Where(x => x.MediaType == "video" && x.DeletedAt == null);
                        var delVideo = video.Where(vi => !model.VideoUrl.Contains(vi.MediaPath ?? String.Empty));
                        _ciPlatformContext.MissionMedia.RemoveRange(delVideo);

                        var saveVideo = model.VideoUrl.Except(video.Select(x => x.MediaPath));

                        foreach (var item in saveVideo)
                        {
                            var vi = new MissionMedium
                            {
                                MissionId = model.missionId,
                                MediaName = "Videos",
                                MediaType = "video",
                                MediaPath = item,
                            };
                            _ciPlatformContext.MissionMedia.Add(vi);


                        }
                        _ciPlatformContext.SaveChanges();
                    }
                    if (model.docList.Count > 0)
                    {
                        foreach (var file in model.docList)
                        {
                            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                            string uploadfile = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\media", fileName);

                            using (var filestream = new FileStream(uploadfile, FileMode.Create))
                            {
                                file.CopyTo(filestream);
                            }
                            //save images to DB

                            MissionDocument missionDocument = new MissionDocument();
                            missionDocument.MissionId = model.missionId;
                            missionDocument.DocumentType = Path.GetExtension(file.FileName);
                            missionDocument.DocumentPath = fileName;
                            missionDocument.DocumentName = file.FileName;
                            _ciPlatformContext.MissionDocuments.Add(missionDocument);
                            _ciPlatformContext.SaveChanges();

                        }
                    }

                    if (model.delDocList.Count > 0)
                    {

                        foreach (var deldoc in model.delDocList)
                        {
                            string uploadfile = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\media", deldoc);
                            var del = _ciPlatformContext.MissionDocuments.FirstOrDefault(x => x.DocumentPath == deldoc && x.MissionId == model.missionId);
                            if (del != null)
                            {
                                _ciPlatformContext.MissionDocuments.Remove(del);
                                _ciPlatformContext.SaveChanges();
                            }
                            if (File.Exists(uploadfile))
                            {
                                File.Delete(uploadfile);
                            }
                        }
                        _ciPlatformContext.SaveChanges();
                    }


                }
            }
        }

        public void DeleteMissionbyAdmin(long missionid)
        {
            var mission = _ciPlatformContext.Missions.FirstOrDefault(m => m.MissionId == missionid);
            if (mission != null)
            {
                var storyIds = _ciPlatformContext.Stories.Where(s => s.MissionId == missionid).Select(s => s.StoryId).ToList();
                if (storyIds.Any())
                {
                    foreach (var storyId in storyIds)
                    {
                        var story = _ciPlatformContext.Stories.FirstOrDefault(s => s.StoryId == storyId);
                        if (story != null)
                        {
                            story.DeletedAt = DateTime.Now;
                            _ciPlatformContext.Stories.Update(story);
                        }
                    }
                    _ciPlatformContext.SaveChanges();
                }
                mission.DeletedAt = DateTime.Now;
                _ciPlatformContext.Missions.Update(mission);
                _ciPlatformContext.SaveChanges();
            }
        }

        public MissionAdminModel FetchMissionForAdmin(long missionid)
        {
            var mission = from m in _ciPlatformContext.Missions
                          where m.MissionId == missionid && m.DeletedAt == null
                          select new MissionAdminModel
                          {
                                 missionName = m.Title,
                                 missionId = m.MissionId,
                                 missionCityId = m.CityId,
                                 missionCountryId = m.CountryId,
                                 missionThemeId = m.ThemeId,
                                 missionType = m.MissionType,
                                 seats = m.TotalMission ?? 0,
                                 description = m.Description ?? string.Empty,
                                 shortDescription = m.ShortDescription ?? string.Empty,
                                 startDate = m.StartDate ?? DateTime.Now,
                                 endDate = m.EndDate ?? DateTime.Now,
                                 deadline = m.Deadline ?? DateTime.Now,
                                 organizationName = m.OrganizationName ?? string.Empty,
                                 organizationDetails = m.OrganizationDetail ?? string.Empty,
                                 Msn_Goal_Action = m.GoalMissions.Select(value => value.GoalValue).FirstOrDefault(),
                                 Msn_Goal_Obj = m.GoalMissions.Select(Obj => Obj.GoalObjectiveText).FirstOrDefault() ?? String.Empty,
                                 VideoUrl = _ciPlatformContext.MissionMedia.Where(m => m.MissionId == missionid && m.MediaType == "video" ).Select(m => m.MediaPath ?? string.Empty).ToList(),
                                 mimgList = _ciPlatformContext.MissionMedia.Where(m => m.MissionId == missionid && m.MediaType != "video").Select(m => m.MediaPath ?? string.Empty).ToList(),
                                 missionDoc= _ciPlatformContext.MissionDocuments.Where(d => d.MissionId == missionid).Select(d => new MissionDocumentDetailsModel
                                 {
                                     docNameList = d.DocumentName ?? String.Empty,
                                     docPathList = d.DocumentPath ?? String.Empty,
                                 }).ToList(),
                                 skillIds = _ciPlatformContext.MissionSkills.Where(s => s.MissionId == missionid && s.DeletedAt == null).Select(s => s.SkillId).ToList(),
                                 skillNames = _ciPlatformContext.MissionSkills.Where(s => s.MissionId == missionid && s.DeletedAt == null).Select(s => s.Skill.SkillName).ToList(),

                          };
            
                return mission.FirstOrDefault() ?? new MissionAdminModel();
               
            

        }
        #endregion

        /*  -----------------------------------    Banner tab     ---------------------------------------------- */

        #region Banner CRUD
        public void BannerCRUDbyAdmin(BannerAdminModel model)

        {
            if (model.BannerAddEditId == 0)
            {
                string fileName;
                

                if (model.BannerImage != null)
                {
                    fileName = Guid.NewGuid().ToString() + Path.GetExtension((model.BannerImage.FileName));
                    string mediaPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\bannerImages");

                    if (!Directory.Exists(mediaPath))
                        Directory.CreateDirectory(mediaPath);

                    string uploadfile = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\bannerImages", fileName);
                    using (var filestream = new FileStream(uploadfile, FileMode.Create))
                    {
                        model.BannerImage.CopyTo(filestream);
                    }
                }
                else
                {
                    fileName = "";
                }

                var result = new Banner
                {
                    Text = model.BannerText,
                    SortOrder = model.BannerSortOrder,
                    Image = fileName ?? string.Empty
                };
                _ciPlatformContext.Banners.Add(result);
                _ciPlatformContext.SaveChanges();
            }
            else
            {
               
                var result = _ciPlatformContext.Banners.FirstOrDefault(x => x.BannerId == model.BannerAddEditId);
                if (result == null)
                {
                    result = new Banner();
                }
                if (model.BannerImage != null)
                {

                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.BannerImage.FileName);
                    string uploadfile = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\bannerImages", fileName);

                    using (var filestream = new FileStream(uploadfile, FileMode.Create))
                    {
                        model.BannerImage.CopyTo(filestream);
                    }
                    //save images to DB
                    result.Image = fileName;

                }
                        result.Text = model.BannerText;
                        result.SortOrder = model.BannerSortOrder;
                        _ciPlatformContext.Banners.Update(result);
                        _ciPlatformContext.SaveChanges();
                    
                }

            }

        public Banner FetchingBanner(long bannerId)
        {
            var result = _ciPlatformContext.Banners.FirstOrDefault(b => b.BannerId == bannerId);
            if (result == null)
            {
                result = new Banner();
            }
            return result;
        }

        public void DeleteBannerbyAdmin(long bannerid)
        {
            var result = _ciPlatformContext.Banners.FirstOrDefault(mission => mission.BannerId == bannerid);
            if (result != null)
            {

                result.DeletedAt = DateTime.Now;
                _ciPlatformContext.Banners.Update(result);
                _ciPlatformContext.SaveChanges();
            }
        }
        #endregion
    }
}
