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
        public IActionResult Mission(string searchString, int? pageNumber)
        {


            if (HttpContext.Session.GetString("role") is null)
            {
                return RedirectToAction("Home", "Home");

            }
            const int pageSize = 10;

            // Get all Mission
            CI.Models.ViewModels.AdminMission adminMission = allRepository.AdminMission.GetAllMissions();

            adminMission.skills = allRepository.AdminMission.GetAllSkil();
            adminMission.themes = allRepository.AdminMission.GetAllTheme();
            adminMission.Missionmedia = allRepository.AdminMission.GetAllMedia();
            adminMission.Country = allRepository.AdminMission.GetAllCountry();
            adminMission.Cities = allRepository.AdminMission.GetAllCities();
            adminMission.missionDocuments = allRepository.AdminMission.GetAllDocumet();

            // Search for Mission if search parameter is provided
            if (!string.IsNullOrEmpty(searchString))
            {
                adminMission.Missions = adminMission.Missions.Where(b => b.Title.ToLower().Contains(searchString)).ToList();
            }

            // Paginate the Mission using the requested page number and page size
            adminMission.TotalPages = (int)Math.Ceiling(adminMission.Missions.Count / (double)pageSize);
            adminMission.PageNumber = pageNumber ?? 1;
            adminMission.Missions = adminMission.Missions.Skip((adminMission.PageNumber - 1) * pageSize).Take(pageSize).ToList();

            adminMission.SearchString = searchString;

            return View(adminMission);
        }

     


        [HttpPost]
        public async Task<IActionResult> AddMission(long id, AdminMission model, List<IFormFile> files, List<IFormFile> fileInput)
        {
            if (HttpContext.Session.GetString("role") is null)
            {
                return RedirectToAction("Home", "Home");

            }
            if (model.MissionType == "TIME")
            {
                ModelState.Remove("GoalValue");
                ModelState.Remove("GoalObjectiveText");
            }



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
                        mission.MissionId = model.MissionId;
                        mission.GoalObject = model.GoalObjectiveText;

                        if (model.MissionType == "GOAL")
                        {
                            GoalMission goalMission = new GoalMission();
                            goalMission.GoalValue = model.GoalValue;
                            goalMission.GoalObjectiveText = model.GoalObjectiveText;
                            mission.GoalMissions.Add(goalMission);
                        }
                        if (model.SelectedSkills != null)
                        {
                            foreach (var skillId in model.SelectedSkills)
                            {
                                MissionSkill skill = new MissionSkill
                                {
                                    SkillId = skillId
                                };
                                mission.MissionSkills.Add(skill);
                            }

                        }

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



                        if (fileInput is not null)
                        {
                            foreach (var document in mission.MissionDocuments)
                            {
                                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "documents", document.DocumentPath);
                                if (System.IO.File.Exists(path))
                                {
                                    System.IO.File.Delete(path);
                                }

                            }
                            int d_count = 1;
                            foreach (var document in fileInput)
                            {
                                FileInfo fileInfo = new FileInfo(document.FileName);
                                string filename = $"mission{mission.MissionId}-document-{d_count}" + fileInfo.Extension;
                                string rootpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "documents", filename);


                                MissionDocument doc = new MissionDocument();
                                {
                                    doc.MissionId = mission.MissionId;
                                    doc.DocumentPath = filename;
                                    doc.DocumentName = document.FileName;
                                }
                                allRepository.AdminMission.AddDoc(doc);

                                using (Stream fileStream = new FileStream(rootpath, FileMode.Create))
                                {
                                    document.CopyTo(fileStream);
                                }
                                d_count++;
                            }
                        }




                    }
                
                return RedirectToAction("Mission", "Mission");

            }

            if (model.MissionId != null)
            {
                if (ModelState.IsValid)
                {

                    data.MissionType = model.MissionType;
                    data.MissionId = id;
                    data.OrganizationName = model.OrganizationName;
                    data.OrganizationDetail = model.OrganizationDetail;
                    data.StartDate = model.StartDate;
                    data.EndDate = model.EndDate;
                    data.Description = model.Description;
                    data.Title = model.Title;
                    data.Deadline = model.Deadline;
                    data.TotalSeats = model.TotalSeats;
                    data.CountryId = model.CountryId;
                    data.CityId = model.CityId;
                    data.Availability = model.Availability;
                    data.ThemeId = model.MissionThemeId;
                    if (model.SelectedSkills != null)
                    {
                        foreach (var skillId in model.SelectedSkills)
                        {

                            if (!data.MissionSkills.Any(ms => ms.SkillId == skillId))
                            {
                                MissionSkill skill = new MissionSkill
                                {
                                    SkillId = skillId
                                };
                                data.MissionSkills.Add(skill);
                            }
                        }

                    }

                    foreach (var file in files)
                    {
                        if (file.Length > 0)
                        {
                            using var stream = new MemoryStream();
                            await file.CopyToAsync(stream);

                            MissionMedia missionMedia = new MissionMedia();
                            {
                                missionMedia.MissionId = id;
                                missionMedia.MediaType = "image";
                                missionMedia.MediaPath = "data:image/png;base64," + Convert.ToBase64String(stream.ToArray());
                            }

                            allRepository.AdminMission.savemedia(missionMedia);
                        }
                    }

                    if (fileInput is not null)
                    {
                        //foreach (var document in model.missionDocuments)
                        //{
                        //    string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "documents", document.DocumentPath);
                        //    if (System.IO.File.Exists(path))
                        //    {
                        //        System.IO.File.Delete(path);
                        //    }

                        //}
                        int d_count = 1;
                        foreach (var document in fileInput)
                        {
                            FileInfo fileInfo = new FileInfo(document.FileName);
                            string filename = $"mission{id}-document-{d_count}" + fileInfo.Extension;
                            string rootpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "documents", filename);


                            MissionDocument doc = new MissionDocument();
                            {
                                doc.MissionId = id;
                                doc.DocumentPath = filename;
                                doc.DocumentName = document.FileName;
                            }
                            allRepository.AdminMission.AddDoc(doc);

                            using (Stream fileStream = new FileStream(rootpath, FileMode.Create))
                            {
                                document.CopyTo(fileStream);
                            }
                            d_count++;
                        }
                    }


                    if (model.MissionType == "GOAL")
                    {
                        GoalMission goalMission = allRepository.AdminMission.getGoalMissionByMissionId(id);
                        goalMission.GoalValue = model.GoalValue;
                        goalMission.GoalObjectiveText = model.GoalObjectiveText;

                        allRepository.AdminMission.UpdateGoalMission(goalMission);
                    }
                    allRepository.AdminMission.UpdateMission(data);

                    return RedirectToAction("Mission", "Mission");
                }
                else
                {
                    //model.Missions = allRepository.AdminMission.GetAllMission();
                    model.MissionId = id;
                    model.skills = allRepository.AdminMission.GetAllSkil();
                    model.themes = allRepository.AdminMission.GetAllTheme();
                    model.Country = allRepository.AdminMission.GetAllCountry();
                    model.Cities = allRepository.AdminMission.GetAllCities();
                    //model.Missionmedia=allRepository.AdminMission.GetAllMedia();
                    model.Missionmedia = data.MissionMedia.Where(x => x.MissionId == id).ToList();
                    model.missionDocuments = data.MissionDocuments.Where(x => x.MissionId == id).ToList();

                    ViewData["Title"] = "Edit Mission";

                    return View("EditMission", model);



                }
            }
            else
            {
                return RedirectToAction("Mission", "Mission");

            }

        }
        [HttpPost]
        public IActionResult GetEditMission(long id)
        {
            if (HttpContext.Session.GetString("role") is null)
            {
                return RedirectToAction("Home", "Home");

            }

            var data = allRepository.AdminMission.GetMissionById(id);
            if (data != null)
            {
                AdminMission adminMission = new AdminMission();
                {
                    if (adminMission.MissionType == "TIME")
                    {
                        ModelState.Remove("GoalValue");
                        ModelState.Remove("GoalObjectiveText");
                    }
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
                    adminMission.Missionmedia = data.MissionMedia.Where(x => x.MissionId == id).ToList();
                    adminMission.missionDocuments = data.MissionDocuments.Where(x => x.MissionId == id).ToList();
                    adminMission.missionSkills = data.MissionSkills.ToList();
                    adminMission.SelectedSkills = data.MissionSkills.Where(x => x.MissionId == id && x.SkillId != null).Select(x => x.SkillId.Value).ToArray();


                    if (adminMission.MissionType == "GOAL")
                    {
                        GoalMission goalMission = data.GoalMissions.FirstOrDefault(x => x.MissionId == id);
                        adminMission.GoalValue = goalMission.GoalValue;
                        adminMission.GoalObjectiveText = goalMission.GoalObjectiveText;
                    }

                }
                adminMission.Missions = allRepository.AdminMission.GetAllMission();
                adminMission.skills = allRepository.AdminMission.GetAllSkil();
                adminMission.themes = allRepository.AdminMission.GetAllTheme();
                adminMission.Country = allRepository.AdminMission.GetAllCountry();
                adminMission.Cities = allRepository.AdminMission.GetAllCities();
                ViewData["Title"] = "Edit Mission";

                //return View("EditMission", null, adminMission);
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

        public IActionResult Declinedocument(long id)
        {
            if (id != 0)
            {
                var record = allRepository.AdminMission.GetDocumentById(id);
                allRepository.AdminMission.DeleteDocument(record);

                return RedirectToAction("Mission", "Mission");
            }
            return View(null);
        }
        public IActionResult Declinemedia(long id)
        {
            if (id != 0)
            {
                var record = allRepository.AdminMission.GetMediaById(id);
                allRepository.AdminMission.DeleteMedia(record);

                return RedirectToAction("Mission", "Mission");
            }
            return View(null);
        }


        //theme//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public IActionResult MissionTheme(string searchString, int page)
        {
            if (HttpContext.Session.GetString("role") is null)
            {
                return RedirectToAction("Home", "Home");

            }
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



        }
        [HttpPost]
        public IActionResult ThemeDecline(long id, int flag, string title)
        {
            if (HttpContext.Session.GetString("role") is null)
            {
                return RedirectToAction("Home", "Home");
            }

            if (string.IsNullOrWhiteSpace(title))
            {
                ViewData["ThemeDecline"] = "ThemeEmpty";

            }
            else if (id == 0 && allRepository.AdminMission.ThemeExists(title))
            {
                ViewData["ThemeDecline"] = "ThemeExit";

            }
            else
            {
                if (id != 0)
                {
                    if (flag == 0)
                    {

                        var record = allRepository.AdminMission.GetThemeById(id);
                        allRepository.AdminMission.GetThemeByMissionId(id);
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
                else
                {
                    CI.Models.MissionTheme missiontheme = new CI.Models.MissionTheme()
                    {
                        Title = title,
                        Status = 1,
                        CreatedAt = DateTime.Now,
                    };
                    allRepository.AdminMission.AddTheme(missiontheme);
                }
            }



            return RedirectToAction("MissionTheme", "Mission");
        }




        ///skill //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public IActionResult MissionSkill(string searchString, int page)
        {
            if (HttpContext.Session.GetString("role") is null)
            {
                return RedirectToAction("Home", "Home");

            }

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


        }

        [HttpPost]
        public IActionResult SkillDecline(long id, int flag, string skillname)
        {
            if (HttpContext.Session.GetString("role") is null)
            {
                return RedirectToAction("Home", "Home");
            }

            if (string.IsNullOrWhiteSpace(skillname))
            {
                ViewData["SkillDecline"] = "SkillEmpty";
            }
            else if (id == 0 && allRepository.AdminMission.SkillExists(skillname))
            {
                ViewData["SkillDecline"] = "SkillExit";
            }
            else
            {
                if (id != 0)
                {
                    if (flag == 0)
                    {

                        var record = allRepository.AdminMission.GetSkillById(id);
                        record.SkillName = skillname;
                        record.Status = 0;
                        allRepository.AdminMission.DeclineSkill(record);
                        allRepository.AdminMission.GetMissionSkills(id);
                    }
                    if (flag == 1)
                    {
                        var record = allRepository.AdminMission.GetSkillById(id);
                        record.SkillName = skillname;
                        record.Status = 1;
                        allRepository.AdminMission.DeclineSkill(record);

                    }

                }
                else
                {
                    CI.Models.Skill skill = new CI.Models.Skill()
                    {
                        SkillName = skillname,
                        Status = 1,
                        CreatedAt = DateTime.Now,
                    };

                    allRepository.AdminMission.AddSkill(skill);
                }
            }



            return RedirectToAction("MissionSkill", "Mission");
        }





        /// ///////////////////////////////////////////////mission application ///////////////////////////////////////////////////////////////////



        public IActionResult MissionApplication(string searchString, int page)
        {
            if (HttpContext.Session.GetString("role") is null)
            {
                return RedirectToAction("Home", "Home");

            }
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



        }
        [HttpPost]
        public IActionResult Decline(long id, int flag)
        {
            if (HttpContext.Session.GetString("role") is null)
            {
                return RedirectToAction("Home", "Home");

            }
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
