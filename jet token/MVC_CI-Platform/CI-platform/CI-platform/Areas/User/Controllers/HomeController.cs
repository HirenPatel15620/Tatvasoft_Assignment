using CI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using CI.Repository;
using CI.Repository.Repository.IRepository;
using CI_platform.Areas.User.Controllers;
using System.Security.Claims;
using CI.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;

namespace CI_platform.Controllers
{
    //[Authorize(Policy = "UserOnly")]
    [Area("User")]
    public class HomeController : Controller
    {
        private readonly IAllRepository allRepository;
        public HomeController(IAllRepository _allRepository)
        {
            allRepository = _allRepository;
        }


        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        [Route("Home")]

        public IActionResult home()
        {
            var useridClaim = HttpContext.User?.FindFirst("UserId");
            if (useridClaim != null)
            {
                int id = JsonSerializer.Deserialize<int>(useridClaim.Value);





                // int id = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid).Value);
                ViewBag.notification = allRepository.Mission.GetUnreadNotification(id).Count();
                ViewData["home"] = "true";

            }

                // Check if city ID is present in session
                if (HttpContext.Session.GetString("Country") is not null)
                {
                    long user_id = long.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid).Value);
                    CI.Models.ViewModels.Mission missions = allRepository.Mission.GetAllMission(long.Parse(HttpContext.Session.GetString("City")));
                    return View(missions);
                }
                else
                {
                    return RedirectToAction("profile", "home");
                }
            
        }
        [HttpPost]
        [Route("Home")]
        public JsonResult home(List<string> countries, List<string> cities, List<string> themes, List<string> skills, string key, string sort_by, int page_index,
            long user_id, long mission_id,string explore)

        {
            var useridClaim = HttpContext.User?.FindFirst("UserId");
            if (useridClaim != null)
            {
                if (key is not null)
                {
                    CI.Models.ViewModels.Mission search_missions = allRepository.Mission.GetSearchMissions(key);
                    var filtered_missions = this.RenderViewAsync("mission_partial", search_missions, true);
                    return Json(new { mission = filtered_missions, success = true, length = search_missions.Missions.Count });
                }

                else if (mission_id != 0)
                {
                    bool success = allRepository.Mission.add_to_favourite(user_id, mission_id);
                    return Json(new { success = success });


                }

                else if (page_index != 0)
                {
                    CI.Models.ViewModels.Mission missions = allRepository.Mission.GetFilteredMissions(countries, cities, themes, skills, sort_by, user_id, explore);
                    var page_missions = this.RenderViewAsync("mission_partial", missions, true);
                    return Json(new { mission = page_missions, length = missions.Missions.Count });
                }
                else
                {
                    CI.Models.ViewModels.Mission missions = allRepository.Mission.GetFilteredMissions(countries, cities, themes, skills, sort_by, user_id, explore);
                    var Cities = this.RenderViewAsync("City_partial", missions, true);
                    var filtered_missions = this.RenderViewAsync("mission_partial", missions, true);
                    return Json(new { mission = filtered_missions, city = Cities, success = true, length = missions.Missions.Count });
                }
            }
            else
            {
                return null;
            }
        }


        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
 
        [Route("volunteering_mission/{id}")]
        public IActionResult volunteering_mission(int id)
        {

            //, string returnUrl = null
            //returnUrl ??= Url.Content("https://localhost:44334/volunteering_mission/{id}");
            var useridClaim = HttpContext.User?.FindFirst("UserId");
            if (useridClaim != null)
            {
                CI.Models.ViewModels.Volunteer_Mission mission = allRepository.Mission.Mission(id, long.Parse(@User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value));

                if (mission is not null)
                {
                    return View(mission);
                    //return Redirect(returnUrl);
                }
                else
                {
                    return View("page_not_found");
                }
            }
            else
            {
                return RedirectToAction("login", "userAuthentication", new { ReturnUrl = $"volunteering_mission/{id}" });
            }
        }
        [HttpPost]
        [Route("volunteering_mission/{id}")]
        public JsonResult volunteering_mission(long User_id, long Mission_id, string comment, int length, string request_for, int count, int rating, List<long> co_workers)
        {
            var useridClaim = HttpContext.User?.FindFirst("UserId");
            if (useridClaim != null)
            {
                if (request_for == "mission")
                {
                    bool success = allRepository.Mission.apply_for_mission(User_id, Mission_id);
                    return Json(new { success = success });
                }
                else if (request_for == "next_volunteers")
                {
                    CI.Models.ViewModels.Volunteer_Mission mission = allRepository.Mission.Next_Volunteers(count, User_id, Mission_id);
                    var recent_volunteers = this.RenderViewAsync("recent_volunteers_partial", mission, true);
                    return Json(new { recent_volunteers = recent_volunteers, Total_volunteers = mission.Total_volunteers });
                }
                else if (request_for == "add_to_favourite")
                {
                    bool success = allRepository.Mission.add_to_favourite(User_id, Mission_id);
                    return Json(new { success = success });
                }
                else if (request_for == "rating")
                {
                    bool success = allRepository.Mission.Rate_mission(User_id, Mission_id, rating);
                    return Json(new { success = success });
                }
                else if (request_for == "recommend")
                {
                    bool success = allRepository.Mission.Recommend(User_id, Mission_id, co_workers);
                    return Json(new { success = success });
                }
                else
                {
                    IEnumerable<CI.Models.ViewModels.Comment_Viewmodel> comments = allRepository.Mission.comment(User_id, Mission_id, comment, length);
                    var new_comment = this.RenderViewAsync("comment_partial", comments, true);
                    return Json(new { comments = new_comment, success = true });
                }
            }
            else
            {
                return null;
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        [Route("Privacy")]
        [Route("User/Home/Privacy")]
        public IActionResult Privacy()
        {
            var cms = allRepository.Sheet.GetALLPolicies();


            return View(cms);


        }
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        [Route("profile")]
        public IActionResult Profile()
        {
            if (User.Identity.IsAuthenticated)
            {
                long user_id = long.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid).Value);

                CI.Models.ViewModels.ProfileViewModel details = allRepository.Profile.Get_Initial_Details(0, user_id);
                if (details?.user?.CountryId is not null)
                {
                    HttpContext.Session.SetString("Country", details?.user?.CountryId.ToString());
                    HttpContext.Session.SetString("City", details?.user?.CityId.ToString());
                }
                if (details?.user?.Avatar is not null)
                {
                    HttpContext.Session.SetString("Avatar", details?.user?.Avatar);
                }
                return View(details);
            }
            else
            {
                return RedirectToAction("login", "userAuthentication");
            }
        }

        [HttpPost]
        [Route("profile")]
        public IActionResult Profile(ProfileViewModel model, int country, string? oldpassword, string? newpassword)
        {
            long user_id = long.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid).Value);
            ProfileViewModel detail = allRepository.Profile.Get_Initial_Details(0, user_id);

            if (oldpassword is not null && newpassword is not null)
            {
                bool success = allRepository.Profile.Change_Password(oldpassword, newpassword, user_id);
                return Json(new { success });
            }
            else
            {
                if (ModelState.IsValid)
                {
                    if (country != 0)
                    {
                        ProfileViewModel details = allRepository.Profile.Get_Initial_Details(country, user_id);
                        var cities = this.RenderViewAsync("ProfileCity_partial", details, true);
                        return Json(new { cities = cities });
                    }
                    else
                    {

                    }
                    {

                        bool success = allRepository.Profile.Update_Details(model, user_id);

                        if (success is false)
                        {
                            ViewData["Profile"] = "profileError";
                            ProfileViewModel details = allRepository.Profile.Get_Initial_Details(country, user_id);
                            return View(details);
                        }

                        return RedirectToAction("login", "userAuthentication");
                    }

                }
                else
                {
                    if (country != 0)
                    {
                        ProfileViewModel details = allRepository.Profile.Get_Initial_Details(country, user_id);
                        var cities = this.RenderViewAsync("ProfileCity_partial", details, true);
                        return Json(new { cities = cities });
                    }
                    else
                    {
                        ProfileViewModel details = allRepository.Profile.Get_Initial_Details(0, user_id);
                        return View(details);
                    }
                }
            }
        }
        [HttpPost]
        public IActionResult contactus(string subject, string message)
        {
            var identity = User.Identity as ClaimsIdentity;
            long user_id = long.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid).Value);

            if (subject != null && message != null)
            {
                bool success = allRepository.Profile.contactus(subject, message, user_id);
                return Json(new { success });
            }

            return View();
        }
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        [Route("Volunteering_Timesheet")]
        public IActionResult Volunteering_Timesheet()
        {
            var identity = User.Identity as ClaimsIdentity;
            var userid = identity?.FindFirst(ClaimTypes.Sid)?.Value;
            VMTimeSheet vMVolunteering = new VMTimeSheet();
            vMVolunteering.timesheets = allRepository.Sheet.GetAllTimeSheetRecordsByUser(userid);
            vMVolunteering.missionApplicatoinsByTime = allRepository.Sheet.GetTimetypeMissionsByUserId(userid);
            vMVolunteering.missionApplicatoinsByGoal = allRepository.Sheet.GetGoaltypeMissionsByUserId(userid);
            return View(vMVolunteering);
        }

        [HttpPost]
        [Route("Volunteering_Timesheet")]
        public IActionResult Volunteering_Timesheet(VMTimeSheet vMVolunteering)
        {
            var identity = User.Identity as ClaimsIdentity;
            var userid = identity?.FindFirst(ClaimTypes.Sid)?.Value;
            var missiontype = allRepository.Sheet.GetMissionTypeById(Convert.ToInt32(vMVolunteering.timesheet.MissionId));
            if (vMVolunteering.timesheet.TimesheetId == 0)
            {
                Timesheet timesheet = new Timesheet();
                timesheet.UserId = Convert.ToInt32(userid);
                timesheet.MissionId = vMVolunteering.timesheet.MissionId;
                timesheet.DateVolunteered = vMVolunteering.timesheet.DateVolunteered;
                timesheet.Notes = vMVolunteering.timesheet.Notes;
                if (missiontype != "GOAL")
                {
                    var hour = vMVolunteering.hour;
                    var minute = vMVolunteering.minute;
                    if (hour < 24 && minute < 59)
                    {
                        timesheet.Time = TimeSpan.Parse(hour + ":" + minute);

                    }
                    else
                    {
                        ViewData["Volunteering_Timesheet"] = "timevalid";

                    }

                    //timesheet.Time = TimeSpan.Parse(hour + ":" + minute);
                    ViewData["Volunteering_Timesheet"] = "time";
                }
                else
                {
                    timesheet.Action = vMVolunteering.timesheet.Action;
                    ViewData["Volunteering_Timesheet"] = "goal";

                }
                allRepository.Sheet.AddTimeSheetRecords(timesheet);
            }
            else
            {
                var record = allRepository.Sheet.GetTimesheetrecordByTimesheetId(Convert.ToInt32(vMVolunteering.timesheet.TimesheetId));
                record.UserId = Convert.ToInt32(userid);
                record.MissionId = vMVolunteering.timesheet.MissionId;
                record.DateVolunteered = vMVolunteering.timesheet.DateVolunteered;
                record.Notes = vMVolunteering.timesheet.Notes;
                if (missiontype != "GOAL")
                {
                    var hour = vMVolunteering.hour;
                    var minute = vMVolunteering.minute;
                    if (hour < 24 && minute < 59)
                    {
                        record.Time = TimeSpan.Parse(hour + ":" + minute);

                    }
                    else
                    {
                        ViewData["Volunteering_Timesheet"] = "timevalid";

                    }
                }
                else
                {
                    record.Action = vMVolunteering.timesheet.Action;
                }
                allRepository.Sheet.UpdateTimeSheetRecord(record);
                ViewData["Volunteering_Timesheet"] = "edit";
            }
            vMVolunteering.timesheets = allRepository.Sheet.GetAllTimeSheetRecordsByUser(userid);
            vMVolunteering.missionApplicatoinsByTime = allRepository.Sheet.GetTimetypeMissionsByUserId(userid);
            vMVolunteering.missionApplicatoinsByGoal = allRepository.Sheet.GetGoaltypeMissionsByUserId(userid);
            return View(vMVolunteering);
        }
        [HttpPost]
        public IActionResult GetEditData(long id)
        {
            var identity = User.Identity as ClaimsIdentity;
            var userid = identity?.FindFirst(ClaimTypes.Sid)?.Value;
            VMTimeSheet timesheet = new VMTimeSheet();
            if (id == 0)
            {
                timesheet.timesheet = null;
            }
            else
            {
                timesheet.timesheet = allRepository.Sheet.GetTimesheetrecordByTimesheetId(id);
                var missiontype = allRepository.Sheet.GetMissionTypeById(timesheet.timesheet.MissionId);
                if (missiontype == "TIME")
                {
                    timesheet.hour = allRepository.Sheet.GetTimesheetrecordByTimesheetId(id).Time.Value.Hours;
                    timesheet.minute = allRepository.Sheet.GetTimesheetrecordByTimesheetId(id).Time.Value.Minutes;

                }
                else
                {
                    timesheet.timesheet.Action = allRepository.Sheet.GetTimesheetrecordByTimesheetId(id).Action;
                }
            }


            timesheet.missionApplicatoinsByTime = allRepository.Sheet.GetTimetypeMissionsByUserId(userid);
            timesheet.missionApplicatoinsByGoal = allRepository.Sheet.GetGoaltypeMissionsByUserId(userid);
            return PartialView("TimesheetModal", timesheet);
        }


        [HttpPost]
        public IActionResult DeleteTimesheetRecord(int timesheetId)
        {
            var record = allRepository.Sheet.GetTimesheetrecordByTimesheetId(timesheetId);
            allRepository.Sheet.DeleteTimesheetRecord(record);

            return RedirectToAction("Volunteering_Timesheet", "Home");
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }



            
        [HttpPost]
        [Route("/home/GetNotificationofUser")]
        public IActionResult GetNotificationofUser()
        {
            var identity = User.Identity as ClaimsIdentity;
            var userid = Convert.ToInt32(identity?.FindFirst(ClaimTypes.Sid)?.Value);

            CI.Models.ViewModels.notication  vMNotification = new CI.Models.ViewModels.notication();
            vMNotification.NotificationSettings = allRepository.Mission.GetNotificationSettingsById(userid);
            vMNotification.OldUserNotifications = allRepository.Mission.GetOlderNotifications(userid);
            vMNotification.NewUserNotifications = allRepository.Mission.GetNewerNotifications(userid);
            //vMNotification.UserNotifications = allRepository.Mission.GetAllNotification(userid);
            vMNotification.Users = allRepository.Mission.GetAllUsersWithoutInActive();
            //List<UserNotification> usernotification = allRepository.Mission.GetAllNotification(userid);
            vMNotification.LastSeen = allRepository.Mission.UpdateLastseenValue(userid);

            return PartialView("_notification", vMNotification);
        }

        [HttpPost]
        [Route("/home/MakeReadedNotification")]
        public long MakeReadedNotification(long usernotificationid)
        {
            allRepository.Mission.UpdateNotificationStatusById(usernotificationid);
            allRepository.Mission.Save();
            return usernotificationid;
        }
        [HttpPost]
        [Route("/home/SaveNotificationSettings")]
        public void SaveNotificationSettings(string[] settingsarray)
        {
            var identity = User.Identity as ClaimsIdentity;
            var userid = Convert.ToInt32(identity?.FindFirst(ClaimTypes.Sid)?.Value);
            var settingrecord = allRepository.Mission.GetNotificationSettingsById(userid);
            allRepository.Mission.DoAllSettingInactive(settingrecord);
            foreach (var setting in settingsarray)
            {
                switch (setting)
                {
                    case "recommendedmission":
                        settingrecord.RecommendMission = 1;
                        break;
                    case "recommendedstory":
                        settingrecord.RecommendStory = 1;
                        break;
                    case "volunteerhour":
                        settingrecord.VolunteerHour = 1;
                        break;
                    case "volunteergoal":
                        settingrecord.VolunteerGoal = 1;
                        break;
                  
                    case "mystory":
                        settingrecord.StoryApprove = 1;
                        break;
                    case "newmessage":
                        settingrecord.NewMessage = 1;
                        break;
                    case "missionapplication":
                        settingrecord.ApplicationApprove = 1;
                        break;
                    case "news":
                        settingrecord.News = 1;
                        break;
                    case "fromemail":
                        settingrecord.FromMail = 1;
                        break;
                }
            }
            allRepository.Mission.UpdateNotificationSettingsByUser(settingrecord);
            allRepository.Mission.Save();
        }
        [HttpPost]
        [Route("/home/ClearAllNotification")]
        public void ClearAllNotification(int userid)
        {
            allRepository.Mission.DeleteNotificationsByUser(userid);
            allRepository.Mission.Save();
        }


    }
}