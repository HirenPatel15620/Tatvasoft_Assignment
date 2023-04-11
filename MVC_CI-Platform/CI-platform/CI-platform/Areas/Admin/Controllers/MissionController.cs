using Microsoft.AspNetCore.Mvc;
using CI.Repository.Repository.IRepository;

namespace CI_platform.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MissionController : Controller
    {
        private readonly IAllRepository allRepository;

        public MissionController(IAllRepository _allRepository)
        {
            allRepository = _allRepository;
        }

        public IActionResult Mission()
        {
            var allmission = allRepository.AdminMission.GetAllMission();
            return View(allmission);
        }

        public IActionResult MissionTheme()
        {
            return View();
        }

        public IActionResult MissionSkill()
        {
            return View();
        }

        public IActionResult MissionApplication()
        {
            return View();
        }
    }
}
