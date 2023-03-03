using CI_Platform.Models.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CI_PlatformWeb.Areas.Volunteer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        //public IActionResult Index()
        //{
        //    return View();
        //}

        public IActionResult Index()
        {
            //TempData["users"] = HttpContext.Session.GetString("username");
            //ViewData["country"] = _logger.Country.Tolist();
            //ViewData["city"] = _logger.City.ToList();
            //ViewData["theme"] = _logger.MissionThemes.ToList();
            //ViewData["skill"] = _logger.Skills.ToList();
            //ViewData["mission"] = _logger.Missions.ToList();
            return View();
        }



        public IActionResult Privacy()
        {
            return View();
        }

        [Authorize]
        public IActionResult Mission_volunteering()
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