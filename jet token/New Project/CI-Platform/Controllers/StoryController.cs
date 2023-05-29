using CI_Platform.Entities.Models;
using CI_Platform.Entities.ViewModels;
using CI_Platform.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CI_Platform.Controllers
{
    public class StoryController : Controller
    {

        private readonly IStoryRepository _iStoryRepository;
        private readonly IPlatformRepository _iPlatformRepository;

        public StoryController(IStoryRepository iStoryRepository , IPlatformRepository iPlatformRepository)
        {
            _iStoryRepository = iStoryRepository;
            _iPlatformRepository = iPlatformRepository;
        }

        #region Story Listing Page 
        public IActionResult StoryListing()
        {
            var useridClaim = HttpContext.User?.FindFirst("UserId");
            if (useridClaim != null)
            {
                #region Fetching all filter List
                ViewBag.cities = _iPlatformRepository.GetAllCities();
                ViewBag.countries = _iPlatformRepository.GetAllCountries();
                ViewBag.skills = _iPlatformRepository.GetAllSkills();
                ViewBag.themes = _iPlatformRepository.GetAllThemes();
                #endregion

                var story = _iStoryRepository.GetStory(string.Empty,1,3);

                return View(story);
            }
            else
            {
                TempData["Message"] = "Login is Required";
                return RedirectToAction("Login", "User");
            }
        }

        public IActionResult FilterStoryListing(string search, int PageNumber, int PageSize)
        {
            var story = _iStoryRepository.GetStory(search, PageNumber, PageSize);

            return PartialView("_StoryListing" , story);
        }
        #endregion

        #region Story Details Page
        public IActionResult StoryDetails(int id)
        {

                if(id == 0)
                {
                    return RedirectToAction("StoryListing");
                }
                var useridClaim = HttpContext.User?.FindFirst("UserId");
                if (useridClaim != null)
                {
                    int userId = JsonSerializer.Deserialize<int>(useridClaim.Value);

                    ViewBag.user = _iPlatformRepository.GetUserDetails(userId);
                    var storydetails = _iStoryRepository.StoryDetails(id, userId);
                    return View(storydetails);
                }

            else
            {
                HttpContext.Session.SetString("returnUrl", Request.Path.Value + "?id=" + id);
                TempData["Message"] = "Login is Required";
                return RedirectToAction("Login", "User");
            }
        }
        #endregion

        #region Share Your Story Page
        public IActionResult ShareYourStory()
        {
            var useridClaim = HttpContext.User?.FindFirst("UserId");
            if (useridClaim != null)
            {
                int userId = JsonSerializer.Deserialize<int>(useridClaim.Value);

                ViewBag.missionlist = _iStoryRepository.GetAllMission(userId);
                return View();
            }
            else
            {
                TempData["Message"] = "Login is Required";
                return RedirectToAction("Login", "User");
            }
        }
        #endregion

        #region Story Submit by User With PostMethod
        [HttpPost]
        public IActionResult ShareYourStory(Story story)
        {
            if (story.MissionId != 0 )
            {
                var useridClaim = HttpContext.User?.FindFirst("UserId");
                if (useridClaim != null)
                {
                    int userId = JsonSerializer.Deserialize<int>(useridClaim.Value);

                    story.UserId = (int)userId;
                    var check = _iStoryRepository.AddYourStory(story);

                    TempData["Message"] = "Story Shared Successfully";
                    return RedirectToAction("StoryListing");
                }
                else
                {
                    return RedirectToAction("Login", "User");
                }
            }
            else
            {
                TempData["Message"] = "All Fields are Required";
                return RedirectToAction("ShareYourStory");
            }
        }
        #endregion

        #region Save Story as Draft by user
        public bool SaveYourStory(int missionid, string title, DateTime date, string description, List<IFormFile> fileList, List<string>? delImgList, List<string>? VideoUrl)
        {
            if(missionid == 0)
            {
                return false;
            }
            else
            {

                var useridClaim = HttpContext.User?.FindFirst("UserId");
                if (useridClaim != null)
                {
                    int userId = JsonSerializer.Deserialize<int>(useridClaim.Value);

                    _iStoryRepository.SaveYourStory(userId, missionid, title, date, description, fileList, delImgList, VideoUrl);
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        #endregion

        #region Fetch Drafted Story
        public JsonResult DraftSavedStory(int missionid)
        {
            var useridClaim = HttpContext.User?.FindFirst("UserId");
            if (useridClaim != null)
            {
                int userId = JsonSerializer.Deserialize<int>(useridClaim.Value);

                var draftStory = _iStoryRepository.SavedDraftStory(userId, missionid);

                return Json(draftStory);
            }
            else
            {
                return Json(null);

            }
        }
        #endregion

        #region TimeSheet Page
        public IActionResult VolunteeringTimesheet()
        {
            var useridClaim = HttpContext.User?.FindFirst("UserId");
            if (useridClaim != null)
            {
                int userId = JsonSerializer.Deserialize<int>(useridClaim.Value);

                var timeSheet = _iStoryRepository.GetTimesheetData(userId);
                return View(timeSheet);
            }
            else
            {
                return RedirectToAction("Login", "User");

            }
        }
        #endregion

        #region TimeSheet CRUD
        public void EditTimesheet(Timesheet model)
        {
            var useridClaim = HttpContext.User?.FindFirst("UserId");
            if (useridClaim != null)
            {
                int userId = JsonSerializer.Deserialize<int>(useridClaim.Value);

                _iStoryRepository.EditTimesheet(model,userId);
            }
            
        }

        public void DeleteTimesheet(int TimesheetId)
        {
            var useridClaim = HttpContext.User?.FindFirst("UserId");
            if (useridClaim != null)
            {
                int userId = JsonSerializer.Deserialize<int>(useridClaim.Value);

                _iStoryRepository.DeleteTimesheet(userId, TimesheetId);
            }
        }

        public JsonResult MissionDateRange(int missionid)
        {
            var dates = _iStoryRepository.SendMissionDateRange(missionid);

            return Json(dates);
        }
        #endregion

        #region Story Invite by User
        public void SendMail(List<int> userId, int sID)
        {
            var useridClaim = HttpContext.User?.FindFirst("UserId");
            if (useridClaim != null)
            {
                int inviteuser = JsonSerializer.Deserialize<int>(useridClaim.Value);
                _iStoryRepository.SendMailForStoryInvite(userId, sID, inviteuser);
            }
        }
        #endregion
    }
}
