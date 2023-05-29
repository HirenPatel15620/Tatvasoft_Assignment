using CI_Platform.Entities.Models;
using CI_Platform.Entities.ViewModels;
using CI_Platform.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CI_Platform.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IAdminRepository _iadminRepository;

        public AdminController(IAdminRepository iadminRepository)
        {
            _iadminRepository = iadminRepository;
        }


        public IActionResult AdminPanel()
        {
            var user = _iadminRepository.GetUserDataForAdmin(String.Empty);
            var cms = _iadminRepository.GetCMSPageForAdmin(String.Empty);
            var mission = _iadminRepository.GetMissionDataForAdmin(String.Empty);
            var story = _iadminRepository.GetStoryDataForAdmin(String.Empty);
            var mApp = _iadminRepository.GetMissionApplicationForAdmin(String.Empty);
            var mTheme = _iadminRepository.GetMissionThemeForAdmin(string.Empty);
            var mSkill = _iadminRepository.GetSkillForAdmin(string.Empty);
            var skillList = _iadminRepository.GetAllSkillListForMission();
            var themeList = _iadminRepository.GetAllThemeListForMission();
            var countryList = _iadminRepository.GetAllCountryListForMission();
            var banner = _iadminRepository.GetBannerDataForAdmin(String.Empty);
            var adminModel = new AdminModel
            {
                user = user,
                cms = cms,
                missionModel = mission,
                story = story,
                banner = banner,
                missionApplication = mApp,
                missionTheme = mTheme,
                missionSkill = mSkill,
                SkillList = skillList,
                ThemeList = themeList,
                CountryList = countryList,
            };
            

            if (adminModel != null)
                return View(adminModel);
            else
                return View(new AdminModel());
        }

        #region User
        public JsonResult CountryListForAddUserbyAdmin()
        {
            var country = _iadminRepository.GetCountryListForAddUserbyAdmin();
            return Json(country);
        }

        public bool AddUserbyAdmin(UserDetailsModel model)
        {
             var user = _iadminRepository.UserCRUDbyAdmin(model);
            
                return user;
            
        }

        public JsonResult FetchUser(long userid)
        {
            var result = _iadminRepository.FetchUserForEdit(userid);
            return Json(result);
        }

        public void DeleteUserbyAdmin(long userid)
        {
            _iadminRepository.DeleteUserbyAdmin(userid);
        }
        #endregion

        #region Search
        public IActionResult Search(string Search, string Identity)
        {
            if (Identity == "userSearch")
            {
                var users = _iadminRepository.GetUserDataForAdmin(Search);
                return PartialView("_UserAdminPartial", users);
            }
            if (Identity == "cmsSearch")
            {
                var cms = _iadminRepository.GetCMSPageForAdmin(Search);
                return PartialView("_CMSAdminPartial", cms);
            }
            if (Identity == "storySearch")
            {
                var story = _iadminRepository.GetStoryDataForAdmin(Search);
                return PartialView("_StoryAdminPartial", story);
            }
            if (Identity == "missionAppSearch")
            {
                var missionApp = _iadminRepository.GetMissionApplicationForAdmin(Search);
                return PartialView("_MissionApplicationAdminPartial", missionApp);
            }
            if (Identity == "themeSearch")
            {
                var missionTheme = _iadminRepository.GetMissionThemeForAdmin(Search);
                return PartialView("_MissionThemeAdminPartial", missionTheme);
            }
            if (Identity == "skillSearch")
            {
                var missionSkill = _iadminRepository.GetSkillForAdmin(Search);
                return PartialView("_MissionSkillAdminPartial", missionSkill);
            }
            if (Identity == "missionSearch")
            {
                var mission = _iadminRepository.GetMissionDataForAdmin(Search);
                return PartialView("_MissionAdminPartial", mission);
            }
            if (Identity == "bannerSearch")
            {
                var banner = _iadminRepository.GetBannerDataForAdmin(Search);
                return PartialView("_BaneerAdminPartial", banner);
            }
            return BadRequest("invalid identity");
        }
        #endregion

        #region CMS
        public void AddCMSbyAdmin(CmsPage model)
        {
            _iadminRepository.CmsCRUDbyAdmin(model);
        }

        public JsonResult FetchCMS(long cmsId)
        {
            var cms = _iadminRepository.FetchCMSForAdmin(cmsId);
            return Json(cms);
        }

        public void DeleteCMSbyAdmin(long cmsid)
        {
            _iadminRepository.CMSDeletebyAdmin(cmsid);
        }
        #endregion

        #region Story
        public void StoryApprovedbyAdmin(long storyid)
        {
            _iadminRepository.StoryApprovedbyAdmin(storyid);
        }

        public void StoryRejectbyAdmin(long storyid)
        {
            _iadminRepository.StoryRejectedbyAdmin(storyid);
        }

        public void StoryDeletedbyAdmin(long storyid)
        {
            _iadminRepository.StoryDeletedbyAdmin(storyid);
        }

        public IActionResult ViewStorybyAdmin(long storyid)
        {
            var story = _iadminRepository.GetStoryDetailsbyAdmin(storyid);
            return PartialView("_ViewStoryAdminPartial", story);
        }
        #endregion

        #region Mission Application
        public void MissionApplicationApprovedbyAdmin(long missionid)
        {
            _iadminRepository.MissionApplicationApprovedbyAdmin(missionid);
        }

        public void MissionApplicationRejectbyAdmin(long missionid)
        {
            _iadminRepository.MissionApplicationRejectbyAdmin(missionid);
        }
        #endregion

        #region Mission Theme
        public void AddThemebyAdmin(MissionTheme model)
        {
            _iadminRepository.ThemeCRUDbyAdmin(model);
        }
        public JsonResult FetchMissionTheme(long themeid)
        {
            var theme = _iadminRepository.FetchMissionThemeForAdmin(themeid);
            return Json(theme);
        }

        public void DeleteMissionThemebyAdmin(long themeid)
        {
            _iadminRepository.DeleteMissionThemebyAdmin(themeid);
        }
        #endregion

        #region Misison Skill
        public void AddSkillbyAdmin(Skill model)
        {
            _iadminRepository.SkillCRUDbyAdmin(model);
        }

        public JsonResult FetchMissionSkill(long skillid)
        {
            var skill = _iadminRepository.FetchMissionSkillForAdmin(skillid);
            return Json(skill);
        }

        public void DeleteMissionSkillbyAdmin(long skillid)
        {
            _iadminRepository.DeleteMissionSkillbyAdmin(skillid);
        }
        #endregion

        #region Mission
        [AllowAnonymous]
        public void AddMissionbyAdmin(MissionAdminModel model)
        {
            _iadminRepository.MissionCRUDbyAdmin(model);
        }

        public void DeleteMissionbyAdmin(long missionid)
        {
            _iadminRepository.DeleteMissionbyAdmin(missionid);
        }

        public JsonResult FetchMission(long missionid)
        {
            var mission = _iadminRepository.FetchMissionForAdmin(missionid);
            return Json(mission);
        }
        #endregion

        #region Banner Management
        public void AddBannerbyAdmin(BannerAdminModel model)
        {
            _iadminRepository.BannerCRUDbyAdmin(model);
        }

        public JsonResult FetchBanner(long BannerId)
        {
            var result = _iadminRepository.FetchingBanner(BannerId);
            return Json(result);
        }

        public void DeleteBannerbyAdmin(long bannerid)
        {
            _iadminRepository.DeleteBannerbyAdmin(bannerid);
        }
        #endregion
    }
}
