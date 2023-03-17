using Microsoft.AspNetCore.Mvc;
using CI.DataAcess.Repository.IRepository;
using System.Security.Claims;
using CI.Models;

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
            return View();
        }
        [Route("stories/share")]
        public IActionResult ShareStory()
        {
            long user_id = long.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid).Value);
            List<Mission> missions = allRepository.Story.Get_User_Missions(user_id);
            return View(missions);
        }
    }
}
