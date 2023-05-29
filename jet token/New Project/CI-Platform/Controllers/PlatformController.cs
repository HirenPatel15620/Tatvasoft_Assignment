using CI_Platform.Entities.ViewModels;
using CI_Platform.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CI_Platform.Controllers
{
    [Authorize(Roles = "User")]
    public class PlatformController : Controller
    {

        private readonly IPlatformRepository _iPlatformRepository;

        public PlatformController(IPlatformRepository iPlatformRepository)
        {
            _iPlatformRepository = iPlatformRepository;
        }

        #region Platform Landing Page
        public IActionResult Platform()
        {
            var useridClaim = HttpContext.User?.FindFirst("UserId");
            if (useridClaim != null)
            {
                int userId = JsonSerializer.Deserialize<int>(useridClaim.Value);
                var cards = _iPlatformRepository.FilterOnMission(new MissionFilter(), userId);

                #region Fetching all filter List
                ViewBag.user = _iPlatformRepository.GetUserDetails((int)userId);
                ViewBag.cities = _iPlatformRepository.GetAllCities();
                ViewBag.countries = _iPlatformRepository.GetAllCountries();
                ViewBag.skills = _iPlatformRepository.GetAllSkills();
                ViewBag.themes = _iPlatformRepository.GetAllThemes();
                #endregion 

                return View(cards);
            }
            else
            {
                TempData["Message"] = "Login is Required";
                return RedirectToAction("Login" , "User");
            }
          
        }
        #endregion

        #region Filter on Missions
        public IActionResult Filter(MissionFilter missionFilter)
        {
            var useridClaim = HttpContext.User?.FindFirst("UserId");
            if (useridClaim != null)
            {
                int userId = JsonSerializer.Deserialize<int>(useridClaim.Value);
                var cards = _iPlatformRepository.FilterOnMission(missionFilter, userId);


                return PartialView("_FilterMissionPartial", cards);
            }
            else
            {
                TempData["Message"] = "Login is Required";
                return RedirectToAction("Login", "User");
            }
        }
        #endregion

        #region Volunteering Page
        [AllowAnonymous]
        public IActionResult Volunteer(int id)
        {
            var useridClaim = HttpContext.User?.FindFirst("UserId");
            if (useridClaim != null)
            {
                int userId = JsonSerializer.Deserialize<int>(useridClaim.Value);


                ViewBag.user = _iPlatformRepository.GetUserDetails((int)userId);
                var oneMission = _iPlatformRepository.GetVolunteerMission(id,(int)userId);

                return View(oneMission);
            }

            else
            {
                HttpContext.Session.SetString("returnUrl", Request.Path.Value + "?id=" + id);
                TempData["Message"] = "Login is Required";
                return RedirectToAction("Login", "User");
            }
        }
        #endregion

        #region Favourite Mission by User
        public void FavouriteMission(int mId , int favcheck)
        {
            var useridClaim = HttpContext.User?.FindFirst("UserId");
            if (useridClaim != null)
            {
                int userId = JsonSerializer.Deserialize<int>(useridClaim.Value);
                if (favcheck == 0)
                {
                    _iPlatformRepository.RemoveFromFavourite((int)userId, mId);
                }
                else
                {
                    _iPlatformRepository.AddToFavourite((int)userId, mId);
                }
            }
        }
        #endregion

        #region Comments by User
        public void Postcomment(int mID , string text)
        {
            var useridClaim = HttpContext.User?.FindFirst("UserId");
            if (useridClaim != null)
            {
                int userId = JsonSerializer.Deserialize<int>(useridClaim.Value);
                _iPlatformRepository.Postcomment((int)userId, mID, text);
            }
        }
        #endregion

        #region Re-Commend to Co-Workers
        public void SendMail(List<int> userId, int mID)
        {
            var useridClaim = HttpContext.User?.FindFirst("UserId");
            if (useridClaim != null)
            {
                int inviteuser = JsonSerializer.Deserialize<int>(useridClaim.Value);
                _iPlatformRepository.SendMail(userId, mID, (int)inviteuser);
            }
        }
        #endregion

        #region Apply For Mission by User
        public void ApplyMission(int mID , int applied)
        {
            var useridClaim = HttpContext.User?.FindFirst("UserId");
            if (useridClaim != null)
            {
                int userId = JsonSerializer.Deserialize<int>(useridClaim.Value);
                if (applied == 0)
                {
                    _iPlatformRepository.ApplyMission((int)userId, mID);
                }
                else
                {
                    _iPlatformRepository.UnapplyMission((int)userId, mID);
                }
            }
        }
        #endregion

        #region Rating by User
        public void UserRating(int missionid,int rating)
        {
            var useridClaim = HttpContext.User?.FindFirst("UserId");
            if (useridClaim != null)
            {
                int userId = JsonSerializer.Deserialize<int>(useridClaim.Value);
                _iPlatformRepository.RatingByUser((int)userId, missionid, rating);
            }
        }
        #endregion

        #region Get All City by Country For Filtering
        public JsonResult CityByCountry(List<long> countryId , List<long> cityId)
        {
            var city = _iPlatformRepository.GetCitiesByCountries(countryId , cityId);   

            return Json(city);
        }
        #endregion

    }
}
