using CI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using CI.DataAcess;
using CI.DataAcess.Repository.IRepository;
using CI.Models.ViewModels;

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
            List<CI.Models.ViewModels.Mission>  missions = allRepository.Mission.GetAllMission();
            return View(missions);
        }
        [HttpPost]
        [Route("Home")]
        public  JsonResult home(List<string> countries, List<string> cities, List<string> themes, List<string> skills,string key,string sort_by)
        {
            if (key is not null)
            {
                List<CI.Models.ViewModels.Mission> search_missions = allRepository.Mission.GetSearchMissions(key);
                return Json(new { missions=search_missions, success = true });
            }
            else
            {
                List<CI.Models.ViewModels.Mission> missions = allRepository.Mission.GetFilteredMissions(countries, cities, themes, skills,sort_by);
                return Json(new { missions, success = true });
            }
        
        }
        [Route("volunteering_mission")]
        public IActionResult volunteering_mission()
        {
            return View();
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

        //public IActionResult Pagination(int page = 1, int pageSize = 10)
        //{
        //    var items = _dbContext.Items.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        //    var totalItems = _dbContext.Items.Count();
        //    var viewModel = new Mission
        //    {
        //        CurrentPage = page,
        //        PageSize = pageSize,
        //        TotalItems = totalItems,
        //        Items = items
        //    };
        //    return View(viewModel);
        //}

    }
}