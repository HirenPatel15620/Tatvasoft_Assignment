using CI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using CI.DataAcess;
using CI.DataAcess.Repository.IRepository;
using CI_platform.Areas.User.Controllers;
using System.Security.Claims;

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
        [Route("Home")]
        public IActionResult home()
        {
            if (User.Identity.IsAuthenticated)
            {
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


        [Route("Privacy")]
        [Route("User/Home/Privacy")]
        public IActionResult Privacy()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View();
            }
            else
            {
                return RedirectToAction("login", "userAuthentication");
            }
        }
        

        [Route("Profile")]
        public IActionResult Profile()
        {

            CI.Models.ViewModels.ProfileViewModel details = allRepository.Profile.Get_Initial_Details(0);
            return View(details);


        }
        [HttpPost]
        [Route("Profile")]
        public JsonResult Profile(int country,CI.Models.ViewModels.ProfileViewModel model)
        {
            long user_id = long.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid).Value);
            if (country != 0)
            {
                bool success = allRepository.Profile.Update_Profile(model, user_id);
                //return RedirectToAction("Profile");
            }


            CI.Models.ViewModels.ProfileViewModel details = allRepository.Profile.Get_Initial_Details(country);
            var cities = this.RenderViewAsync("ProfileCity_partial", details, true);
            return Json(new { cities });

        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}