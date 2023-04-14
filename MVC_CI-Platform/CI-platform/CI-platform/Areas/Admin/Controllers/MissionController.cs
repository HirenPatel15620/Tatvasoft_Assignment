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
            var missiontheme = allRepository.AdminMission.GetAllTheme();
            return View(missiontheme);
        }
        [HttpPost]
        public IActionResult ThemeDecline(long id, int flag,string title)
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
            var missionskill=allRepository.AdminMission.GetAllSkil();
            return View(missionskill);
        }

        [HttpPost]
        public IActionResult SkillDecline(long id, int flag,string skillname)
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
                    record.SkillName=skillname;
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
