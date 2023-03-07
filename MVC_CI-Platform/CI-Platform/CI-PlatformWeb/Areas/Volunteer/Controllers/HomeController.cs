using CI_Platform.Models;
using CI_Platform.Models.Models;
using CI_Platform.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CI_PlatformWeb.Areas.Volunteer.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHomeRepository _homeRepository;

        public HomeController(IHomeRepository homeRepository)
        {
            _homeRepository = homeRepository;
        }

        public IActionResult Index()
        {
            ViewData["country"] = _homeRepository.GetCountries();
            ViewData["city"] = _homeRepository.GetCities();
            ViewData["skill"] = _homeRepository.GetSkills();
            ViewData["theme"] = _homeRepository.GetMissionThemes();
            ViewData["mission"] = _homeRepository.GetMissions();
            ViewData["goalmission"] = _homeRepository.GetGoalMissions();
            ViewData["sumofaction"] = _homeRepository.GetSumOfAction();
            if (TempData["Logout"] != null)
                ViewBag.success = TempData["Logout"];
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Authorize]
        public IActionResult Mission_volunteering()
        {
            ViewData["country"] = _homeRepository.GetCountries();
            ViewData["city"] = _homeRepository.GetCities();
            ViewData["skill"] = _homeRepository.GetSkills();
            ViewData["theme"] = _homeRepository.GetMissionThemes();
            ViewData["mission"] = _homeRepository.GetMissions();
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}