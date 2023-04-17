using Microsoft.AspNetCore.Mvc;
using CI.Repository.Repository.IRepository;
using CI.Models.ViewModels;
using CI.Models;
using System.Security.Claims;

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
            AdminMission adminMission = new AdminMission();
            adminMission.Missions = allRepository.AdminMission.GetAllMission();
            adminMission.skills=allRepository.AdminMission.GetAllSkil();
            adminMission.themes=allRepository.AdminMission.GetAllTheme();
            adminMission.Country = allRepository.AdminMission.GetAllCountry();
            //if (country != 0)
            //{
            //    ProfileViewModel details = allRepository.Profile.Get_Initial_Details(country, user_id);
            //    var cities = this.RenderViewAsync("ProfileCity_partial", details, true);
            //    return Json(new { cities = cities });
            //}
            adminMission.Cities=allRepository.AdminMission.GetAllCities();
            return View(adminMission);
        }



        [HttpPost]
        public IActionResult AddMission(AdminMission model)
       {


            if (ModelState.IsValid)
            {
            CI.Models.Mission mission = new CI.Models.Mission();
                model.Availability = mission.Availability;
                model.Description = mission.Description;
                model.CityId= mission.CityId;
                model.EndDate= mission.EndDate;
                model.StartDate= mission.StartDate;
                model.MissionThemeId = mission.ThemeId;


                return View();

            }
            else
            {
                return View();
            }

        }














        public IActionResult DeleteMission(long id)
        {
            if (id != 0)
            {
                var record = allRepository.AdminMission.GetMissionById(id);
                record.Status = false;
                record.DeletedAt = DateTime.Now;    
                allRepository.AdminMission.DeleteMission(record);

            }
            return RedirectToAction("MissionApplication", "Mission");
        }

        public IActionResult MissionTheme()
        {
            var missiontheme = allRepository.AdminMission.GetAllTheme();
            return View(missiontheme);
        }
        [HttpPost]
        public IActionResult ThemeDecline(long id, int flag, string title)
        {
            if (id != 0)
            {
                if (flag == 0)
                {
                    var record = allRepository.AdminMission.GetThemeById(id);
                    record.Status = 0;
                    record.Title = title;
                    allRepository.AdminMission.DeclineTheme(record);

                }
                if (flag == 1)
                {
                    var record = allRepository.AdminMission.GetThemeById(id);
                    record.Status = 1;
                    record.Title = title;
                    allRepository.AdminMission.DeclineTheme(record);

                }
            }
            if (id == 0)
            {
                CI.Models.MissionTheme missiontheme = new CI.Models.MissionTheme()
                {
                    Title = title,
                    Status = 1,
                    CreatedAt = DateTime.Now,
                };
                allRepository.AdminMission.AddTheme(missiontheme);
            }
            return RedirectToAction("MissionTheme", "Mission");
        }





        ///skill //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public IActionResult MissionSkill()
        {
            var missionskill = allRepository.AdminMission.GetAllSkil();
            return View(missionskill);
        }

        [HttpPost]
        public IActionResult SkillDecline(long id, int flag, string skillname)
        {
            if (id != 0)
            {
                if (flag == 0)
                {
                    var record = allRepository.AdminMission.GetSkillById(id);
                    record.SkillName = skillname;
                    record.Status = 0;
                    allRepository.AdminMission.DeclineSkill(record);

                }
                if (flag == 1)
                {
                    var record = allRepository.AdminMission.GetSkillById(id);
                    record.SkillName = skillname;
                    record.Status = 1;
                    allRepository.AdminMission.DeclineSkill(record);

                }
            }
            if (id == 0)
            {
                CI.Models.Skill skill = new CI.Models.Skill()
                {
                    SkillName = skillname,
                    Status = 1,
                    CreatedAt = DateTime.Now,
                };
                allRepository.AdminMission.AddSkill(skill);
            }
            return RedirectToAction("MissionSkill", "Mission");
        }





        /// ///////////////////////////////////////////////mission application ///////////////////////////////////////////////////////////////////



        public IActionResult MissionApplication()
        {
            var missionapplication = allRepository.AdminMission.GetAllMissionApplication();
            return View(missionapplication);
        }
        [HttpPost]
        public IActionResult Decline(long id, int flag)
        {
            if (id != 0)
            {
                if (flag == 0)
                {
                    var record = allRepository.AdminMission.GetMissionApplicationById(id);
                    record.ApprovalStatus = "DECLINE";
                    allRepository.AdminMission.DeclineUser(record);

                }
                if (flag == 1)
                {
                    var record = allRepository.AdminMission.GetMissionApplicationById(id);
                    record.ApprovalStatus = "APPROVE";
                    allRepository.AdminMission.DeclineUser(record);

                }
            }
            return RedirectToAction("MissionApplication", "Mission");
        }


        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }
}
