using CI_Platform.Entities.Models;
using CI_Platform.Entities.ViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CI_Platform.Repository.Interface
{
    public interface IStoryRepository
    {

        PageList<StoryListingPageModel> GetStory(string keyword, int PageNumber, int PageSize);

        StoryListingPageModel StoryDetails(int storyid, int userId);

        IEnumerable<SelectListItem> GetAllMission(int userId);

        bool AddYourStory(Story story);

        void SaveYourStory(int userid, int missionid, string title, DateTime date, string description, List<IFormFile> fileList, List<string>? delImgList, List<string>? VideoUrl);

        SavedDraftModel SavedDraftStory(int userid, int missionid);

        VolunteeringTimesheetModel GetTimesheetData(int userid);

        void EditTimesheet(Timesheet model, int userid);

        void DeleteTimesheet(int userid, int TimesheetId);

        TimeSheetModel SendMissionDateRange(int missionid);

        void SendMailForStoryInvite(List<int> userid, int storyid, int inviteuser);


    }
}
