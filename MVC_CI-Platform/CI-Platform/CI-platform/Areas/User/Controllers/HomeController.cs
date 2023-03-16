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
            CI.Models.ViewModels.Mission missions = allRepository.Mission.GetAllMission();
            return View(missions);
        }
        [HttpPost]
        [Route("Home")]
        public JsonResult home(List<string> countries, List<string> cities, List<string> themes, List<string> skills, string key, string sort_by, int page_index)
        {
            if (page_index != 0)
            {
                CI.Models.ViewModels.Mission missions = allRepository.Mission.GetFilteredMissions(countries, cities, themes, skills, sort_by, page_index);
                var page_missions = this.RenderViewAsync("mission_partial", missions, true);
                return Json(new { mission = page_missions, length = missions.Missions.Count });
            }
            if (key is not null)
            {
                CI.Models.ViewModels.Mission search_missions = allRepository.Mission.GetSearchMissions(key, page_index);
                var filtered_missions = this.RenderViewAsync("mission_partial", search_missions, true);
                return Json(new { mission = filtered_missions, success = true, length = search_missions.Missions.Count });
            }
            else
            {
                CI.Models.ViewModels.Mission missions = allRepository.Mission.GetFilteredMissions(countries, cities, themes, skills, sort_by, page_index);
                var Cities = this.RenderViewAsync("City_partial", missions, true);
                var filtered_missions = this.RenderViewAsync("mission_partial", missions, true);
                return Json(new { mission = filtered_missions, city = Cities, success = true, length = missions.Missions.Count });
            }
        }














        [Route("volunteering_mission/{id}")]
        public IActionResult volunteering_mission(int id)
        {
            CI.Models.ViewModels.Volunteer_Mission mission = allRepository.Mission.Mission(id, long.Parse(@User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value));
            return View(mission);
        }
        [HttpPost]
        [Route("volunteering_mission/{id}")]
        public JsonResult volunteering_mission(long User_id, long Mission_id, string comment, int length, string request_for, int count, int rating)
        {
            if (request_for == "mission")
            {
                bool success = allRepository.Mission.apply_for_mission(User_id, Mission_id);
                return Json(new { success = success });
            }
            else if (request_for == "next_volunteers")
            {
                CI.Models.ViewModels.Volunteer_Mission mission = allRepository.Mission.Next_Volunteers(count, Mission_id);
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
            else
            {
                IEnumerable<CI.Models.ViewModels.Comment_Viewmodel> comments = allRepository.Mission.comment(User_id, Mission_id, comment, length);
                return Json(new { comments, success = true });
            }
        }
        public IActionResult Privacy()
        {
            return View();
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}