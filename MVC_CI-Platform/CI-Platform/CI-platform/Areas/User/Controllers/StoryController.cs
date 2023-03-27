using Microsoft.AspNetCore.Mvc;
using CI.DataAcess.Repository.IRepository;
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
        public JsonResult StoryListing(int page_index)
        {
            long user_id = long.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid).Value);
           
                CI.Models.ViewModels.Mission stories = allRepository.Story.GetFileredStories(page_index,user_id);
                var next_stories = this.RenderViewAsync("story_partial", stories, true);
                return Json(new { next_stories});
        }


        [Route("stories/share")]
        public IActionResult ShareStory()
        {
            long user_id = long.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid).Value);
            List<Mission> missions = allRepository.Story. Get_User_Missions(user_id);
            return View(missions);
        }
        [HttpPost]
        [Route("stories/share")]
        public JsonResult ShareStory(long story_id,long mission_id, string title,string published_date, string mystory, List<string>media,string type)
        {
            long user_id = long.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid).Value);
            if (type=="PUBLISHED")
            {
                bool success = allRepository.Story.AddStory(user_id,story_id,mission_id, title, published_date, mystory, media,type);
                return Json(new { success });
            }
            else
            {
                bool success = allRepository.Story.AddStory(user_id,0,mission_id, title, published_date, mystory, media,type);
                return Json(new { success });
            }
           
        }
       
        [Route("stories/detail/{id}")]
        public IActionResult StoryDetail(long id)
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
        



    }
}
