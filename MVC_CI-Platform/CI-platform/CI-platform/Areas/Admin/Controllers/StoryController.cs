using CI.Repository.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace CI_platform.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class StoryController : Controller
    {
        private readonly IAllRepository allRepository;

        public StoryController(IAllRepository _allRepository)
        {
            allRepository = _allRepository;
        }
        public IActionResult Story()
        {
            return View();
        }
        public IActionResult Banner()
        {
            return View();
        }
    }
}
