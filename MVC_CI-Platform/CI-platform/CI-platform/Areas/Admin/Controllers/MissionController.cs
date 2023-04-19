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
            //string searchString, int page

            AdminMission adminMission = new AdminMission();
            adminMission.Missions = allRepository.AdminMission.GetAllMission();
            adminMission.skills = allRepository.AdminMission.GetAllSkil();
            adminMission.themes = allRepository.AdminMission.GetAllTheme();
            adminMission.Country = allRepository.AdminMission.GetAllCountry();
            //if (country != 0)
            //{
            //    ProfileViewModel details = allRepository.Profile.Get_Initial_Details(country, user_id);
            //    var cities = this.RenderViewAsync("ProfileCity_partial", details, true);
            //    return Json(new { cities = cities });
            //}
            adminMission.Cities = allRepository.AdminMission.GetAllCities();
            return View(adminMission);

            //int pageSize = 10; // Number of records to display per page

            //IEnumerable<AdminMission> missions;

            //if (!string.IsNullOrEmpty(searchString))
            //{
            //    missions = allRepository.AdminMission.SearchMission(searchString);
            //}
            //else
            //{
            //    missions = allRepository.AdminMission.GetMission();
            //}

            //int totalRecords = missions.Count();
            //int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
            //int skip = (page - 1) * pageSize;

            //missions = missions.Skip(skip).Take(pageSize).ToList();

            //ViewData["SearchString"] = searchString;
            //ViewData["CurrentPage"] = page;

            //ViewData["TotalPages"] = totalPages;

            //return View(missions);

        }



        [HttpPost]
        public async Task<IActionResult> AddMission(long id, AdminMission model, List<IFormFile> files)
        {
            var data = allRepository.AdminMission.GetMissionById(id);
            if (data == null)
            {
                if (ModelState.IsValid)
                {
                    CI.Models.Mission mission = new CI.Models.Mission();
                    mission.Availability = model.Availability;
                    mission.Description = model.Description;
                    mission.CityId = model.CityId;
                    mission.EndDate = model.EndDate;
                    mission.StartDate = model.StartDate;
                    mission.Deadline = model.Deadline;
                    mission.ThemeId = model.MissionThemeId;
                    mission.CountryId = model.CountryId;
                    mission.OrganizationDetail = model.OrganizationDetail;
                    mission.OrganizationName = model.OrganizationName;
                    mission.Title = model.Title;
                    mission.TotalSeats = model.TotalSeats;
                    mission.MissionType = model.MissionType;
                    allRepository.AdminMission.AddMission(mission);

                    long missionid = mission.MissionId;

                    mission.MissionMedia = new List<MissionMedia>();

                    foreach (var file in files)
                    {
                        if (file.Length > 0)
                        {
                            using var stream = new MemoryStream();
                            await file.CopyToAsync(stream);

                            MissionMedia missionMedia = new MissionMedia();
                            {
                                missionMedia.MissionId = missionid;
                                missionMedia.MediaType = "image";
                                missionMedia.MediaPath = "data:image/png;base64," + Convert.ToBase64String(stream.ToArray());
                            }

                            allRepository.AdminMission.savemedia(missionMedia);
                        }
                    }
                }

                return RedirectToAction("Mission", "Mission");

            }
            else
            {
                return RedirectToAction("Mission", "Mission");

            }

        }
        [HttpPost]
        public IActionResult GetEditMission(long id)
        {

            var data = allRepository.AdminMission.GetMissionById(id);
            if (data != null)
            {
                AdminMission adminMission = new AdminMission();
                {

                    adminMission.MissionId = id;
                    adminMission.Availability = data.Availability;
                    adminMission.CityId = data.CityId;
                    adminMission.CountryId = data.CountryId;
                    adminMission.MissionType = data.MissionType;
                    adminMission.Title = data.Title;
                    adminMission.Deadline = data.Deadline;
                    adminMission.Description = data.Description;
                    adminMission.StartDate = data.StartDate;
                    adminMission.EndDate = data.EndDate;
                    adminMission.TotalSeats = data.TotalSeats;
                    adminMission.OrganizationName = data.OrganizationName;
                    adminMission.OrganizationDetail = data.OrganizationDetail;
                    adminMission.MissionThemeId = data.ThemeId;
                    adminMission.SkillId = data.MissionSkills.ToList().Count();


                }
                adminMission.Missions = allRepository.AdminMission.GetAllMission();
                adminMission.skills = allRepository.AdminMission.GetAllSkil();
                adminMission.themes = allRepository.AdminMission.GetAllTheme();
                adminMission.Country = allRepository.AdminMission.GetAllCountry();
                adminMission.Cities = allRepository.AdminMission.GetAllCities();

                return PartialView("EditMission", adminMission);
                //return true;
            }
            else
            {
                return NotFound();
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
            return RedirectToAction("Mission", "Mission");
        }



        //theme//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public IActionResult MissionTheme(string searchString, int page)
        {
            int pageSize = 10; // Number of records to display per page

            IEnumerable<MissionTheme> missionThemes;

            if (!string.IsNullOrEmpty(searchString))
            {
                missionThemes = allRepository.AdminMission.SearchTheme(searchString);
            }
            else
            {
                missionThemes = allRepository.AdminMission.GetTheme();
            }

            int totalRecords = missionThemes.Count();
            int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
            int skip = (page - 1) * pageSize;

            missionThemes = missionThemes.Skip(skip).Take(pageSize).ToList();

            ViewData["SearchString"] = searchString;
            ViewData["CurrentPage"] = page;

            ViewData["TotalPages"] = totalPages;

            return View(missionThemes);
            //var missiontheme = allRepository.AdminMission.GetAllTheme();
            //return View(missiontheme);


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

                    //var missionrecord=allRepository.AdminMission.GetThemeByMissionId(id);
                    //missionrecord.ThemeId = null;
                    //allRepository.AdminMission.DeclineThemeInMission(missionrecord);

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

        public IActionResult MissionSkill(string searchString, int page)
        {

            int pageSize = 10; // Number of records to display per page

            IEnumerable<Skill> missionSkills;

            if (!string.IsNullOrEmpty(searchString))
            {
                missionSkills = allRepository.AdminMission.SearchSkill(searchString);
            }
            else
            {
                missionSkills = allRepository.AdminMission.GetSkill();
            }

            int totalRecords = missionSkills.Count();
            int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
            int skip = (page - 1) * pageSize;

            missionSkills = missionSkills.Skip(skip).Take(pageSize).ToList();

            ViewData["SearchString"] = searchString;
            ViewData["CurrentPage"] = page;

            ViewData["TotalPages"] = totalPages;

            return View(missionSkills);

            //var missionskill = allRepository.AdminMission.GetAllSkil();
            //return View(missionskill);
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



        public IActionResult MissionApplication(string searchString, int page)
        {

            int pageSize = 10; // Number of records to display per page

            IEnumerable<MissionApplication> missionApplications;

            if (!string.IsNullOrEmpty(searchString))
            {
                missionApplications = allRepository.AdminMission.SearchMissionApplication(searchString);
            }
            else
            {
                missionApplications = allRepository.AdminMission.GetMissionApplication();
            }

            int totalRecords = missionApplications.Count();
            int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
            int skip = (page - 1) * pageSize;

            missionApplications = missionApplications.Skip(skip).Take(pageSize).ToList();

            ViewData["SearchString"] = searchString;
            ViewData["CurrentPage"] = page;

            ViewData["TotalPages"] = totalPages;

            return View(missionApplications);



            //var missionapplication = allRepository.AdminMission.GetAllMissionApplication();
            //return View(missionapplication);
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
