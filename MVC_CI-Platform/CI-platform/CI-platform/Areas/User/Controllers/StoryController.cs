using Microsoft.AspNetCore.Mvc;
using CI.Repository.Repository.IRepository;
using System.Security.Claims;
using CI.Models;
using CI_platform.Areas.User.Controllers;

namespace CI_platform.Controllers
{
    [Area("User")]
    public class StoryController : Controller
    {
        private readonly IAllRepository allRepository;
        public StoryController(IAllRepository _allRepository)
        {
            allRepository = _allRepository;
        }



        [Route("stories")]
        public IActionResult StoryListing()
        {
            long user_id = long.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid).Value);
            CI.Models.ViewModels.Mission stories = allRepository.Story.GetStories(user_id);
            return View(stories);
        }

        [HttpPost]
        [Route("stories")]
        public JsonResult StoryListing(int page_index, string key)
        {
            long user_id = long.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid).Value);
            if (key is not null)
            {
                CI.Models.ViewModels.Mission GetSearchStory = allRepository.Story.GetSearchStory(key);
                var filtered_story = this.RenderViewAsync("story_partial", GetSearchStory, true);
                return Json(new { Stories = filtered_story, success = true });
            }


            CI.Models.ViewModels.Mission stories = allRepository.Story.GetFileredStories(page_index, user_id);
            var next_stories = this.RenderViewAsync("story_partial", stories, true);
            return Json(new { next_stories });


        }


        [Route("stories/share")]
        public IActionResult ShareStory()
        {
            long user_id = long.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid).Value);
            List<Mission> missions = allRepository.Story.Get_User_Missions(user_id);
            return View(missions);
            ViewBag.success = "share story successfull";
        }
        [HttpPost]
        [Route("stories/share")]
        public JsonResult ShareStory(long story_id, long mission_id, string title, string published_date, string mystory, List<string> media, string type)
        {
            long user_id = long.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid).Value);
            if (type == "PENDING")
            {
                bool success = allRepository.Story.AddStory(user_id, story_id, mission_id, title, published_date, mystory, media, type);
                return Json(new { success });
                ViewBag.success = "share story  view successfull";

            }
            else
            {
                bool success = allRepository.Story.AddStory(user_id, 0, mission_id, title, published_date, mystory, media, type);
                return Json(new { success });
                ViewBag.success = "share story draft view successfull";

            }

            ViewData["ShareStory"] = "True";
        }

        [Route("stories/detail/{id}")]
        public IActionResult StoryDetail(long id)
        {
            if (User.Identity.IsAuthenticated)
            {
                long user_id = long.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid).Value);
                CI.Models.ViewModels.StoryViewModel story = allRepository.Story.GetStory(user_id, id);
                if (story is not null)
                {
                    allRepository.Story.Add_View(user_id, id);
                    return View(story);
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
        [Route("stories/detail/{id}")]
        public JsonResult StoryDetail(long user_id, long story_id, List<long> co_workers)
        {

            bool success = allRepository.Story.Recommend(user_id, story_id, co_workers);
            return Json(new { success });

        }
    }
}
