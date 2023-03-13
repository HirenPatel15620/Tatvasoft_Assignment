using Microsoft.AspNetCore.Mvc;

namespace CI_platform.Controllers
{
    [Area("User")]
    public class StoryController : Controller
    {
        [Route("stories")]
        public IActionResult storylisting()
        {
            return View();
        }
    }
}
