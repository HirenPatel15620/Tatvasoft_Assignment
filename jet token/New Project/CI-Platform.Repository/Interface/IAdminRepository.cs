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
    public interface IAdminRepository
    {
        List<User> GetUserDataForAdmin(string search);

        List<CmsPage> GetCMSPageForAdmin(string search);

        List<StoryAdminModel> GetStoryDataForAdmin(string search);

        List<Mission> GetMissionDataForAdmin(string search);

        List<MissionTheme> GetMissionThemeForAdmin(string search);

        List<MissionApplicationAdminModel> GetMissionApplicationForAdmin(string search);

        List<Skill> GetSkillForAdmin(string search);

        List<Banner> GetBannerDataForAdmin(string search);

        List<SelectListItem> GetCountryListForAddUserbyAdmin();

        bool UserCRUDbyAdmin(UserDetailsModel model);

        User FetchUserForEdit(long userid);

        void DeleteUserbyAdmin(long userid);

        void CmsCRUDbyAdmin(CmsPage model);

        void CMSDeletebyAdmin(long cmsid);

        CmsPage FetchCMSForAdmin(long cmsId);

        void StoryApprovedbyAdmin(long storyid);

        void StoryRejectedbyAdmin(long storyid);

        void StoryDeletedbyAdmin(long storyid);

        StoryAdminModel GetStoryDetailsbyAdmin(long storyid);

        void MissionApplicationRejectbyAdmin(long missionid);

        void MissionApplicationApprovedbyAdmin(long missionid);

        void ThemeCRUDbyAdmin(MissionTheme model);

        MissionTheme FetchMissionThemeForAdmin(long themeid);

        void DeleteMissionThemebyAdmin(long themeid);

        void SkillCRUDbyAdmin(Skill model);

        Skill FetchMissionSkillForAdmin(long skillid);

        void DeleteMissionSkillbyAdmin(long skillid);

        List<SelectListItem> GetAllSkillListForMission();

        List<SelectListItem> GetAllThemeListForMission();

        List<SelectListItem> GetAllCountryListForMission();

        void MissionCRUDbyAdmin(MissionAdminModel model);

        void DeleteMissionbyAdmin(long missionid);

        MissionAdminModel FetchMissionForAdmin(long missionid);

        void BannerCRUDbyAdmin(BannerAdminModel model);

        Banner FetchingBanner(long bannerId);

        void DeleteBannerbyAdmin(long bannerid);

    }
}
