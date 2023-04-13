using CI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using CI.Repository;
using CI.Repository.Repository.IRepository;
using CI_platform.Areas.User.Controllers;
using System.Security.Claims;
using CI.Models.ViewModels;

namespace CI_platform.Controllers
{
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
            if (User.Identity.IsAuthenticated)
            {
                //return RedirectToAction("Profile", "home");
                ViewData["home"] = "true";
                CI.Models.ViewModels.Mission missions = allRepository.Mission.GetAllMission();
                return View(missions);

            }
            else
            {
                return RedirectToAction("login", "userAuthentication");
            }
        }
        [HttpPost]
        [Route("Home")]
        public JsonResult home(List<string> countries, List<string> cities, List<string> themes, List<string> skills, string key, string request_for, string sort_by, int page_index,
            long user_id, long mission_id, List<long> co_workers)
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
            else if (request_for == "recommend")
            {
                bool successworker = allRepository.Mission.Recommend(user_id, mission_id, co_workers);
                return Json(new { successworker });
            }
            else if (page_index != 0)
            {
                CI.Models.ViewModels.Mission missions = allRepository.Mission.GetFilteredMissions(countries, cities, themes, skills, sort_by, user_id);
                var page_missions = this.RenderViewAsync("mission_partial", missions, true);
                return Json(new { mission = page_missions, length = missions.Missions.Count });
            }
            else
            {
                CI.Models.ViewModels.Mission missions = allRepository.Mission.GetFilteredMissions(countries, cities, themes, skills, sort_by, user_id);
                var Cities = this.RenderViewAsync("City_partial", missions, true);
                var filtered_missions = this.RenderViewAsync("mission_partial", missions, true);
                return Json(new { mission = filtered_missions, city = Cities, success = true, length = missions.Missions.Count });
            }
        }


        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        [Route("volunteering_mission/{id}")]
        public IActionResult volunteering_mission(int id)
        {

            //, string returnUrl = null
            //returnUrl ??= Url.Content("https://localhost:44334/volunteering_mission/{id}");
            if (User.Identity.IsAuthenticated)
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
                return RedirectToAction("login", "userAuthentication");
            }
        }
        [HttpPost]
        [Route("volunteering_mission/{id}")]
        public JsonResult volunteering_mission(long User_id, long Mission_id, string comment, int length, string request_for, int count, int rating, List<long> co_workers)
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
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        [Route("Privacy")]
        [Route("User/Home/Privacy")]
        public IActionResult Privacy()
        {
            var cms=allRepository.Sheet.GetALLPolicies();

            if (User.Identity.IsAuthenticated)
            {
                return View(cms);
            }
            else
            {
                return RedirectToAction("login", "userAuthentication");
            }
        }
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        [Route("profile")]
        public IActionResult Profile()
        {
            long user_id = long.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid).Value); 
            CI.Models.ViewModels.ProfileViewModel details = allRepository.Profile.Get_Initial_Details(0, user_id);
            return View(details);
        }

        [HttpPost]
        [Route("profile")]
        public IActionResult Profile(ProfileViewModel model, int country, string? oldpassword, string? newpassword)
        {
            long user_id = long.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid).Value);
            ProfileViewModel detail = allRepository.Profile.Get_Initial_Details(0,user_id);

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
                        ProfileViewModel details = allRepository.Profile.Get_Initial_Details(country,user_id);
                        var cities = this.RenderViewAsync("ProfileCity_partial", details, true);
                        return Json(new { cities = cities });
                    }
                    else
                    {

                    }
                    {
                     
                        bool success = allRepository.Profile.Update_Details(model, user_id);
                        return RedirectToAction("login", "userAuthentication");
                    }

                }
                else
                {
                    if (country != 0)
                    {
                        ProfileViewModel details = allRepository.Profile.Get_Initial_Details(country,user_id);
                        var cities = this.RenderViewAsync("ProfileCity_partial", details, true);
                        return Json(new { cities = cities });
                    }
                    else
                    {
                        ProfileViewModel details = allRepository.Profile.Get_Initial_Details(0,user_id);
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
                    timesheet.Time = TimeSpan.Parse(hour + ":" + minute);
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
                    record.Time = TimeSpan.Parse(hour + ":" + minute);
                }
                else
                {
                    record.Action = vMVolunteering.timesheet.Action;
                }
                allRepository.Sheet.UpdateTimeSheetRecord(record);
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
            ViewData["GetEditData"] = "edit";

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
    }
}