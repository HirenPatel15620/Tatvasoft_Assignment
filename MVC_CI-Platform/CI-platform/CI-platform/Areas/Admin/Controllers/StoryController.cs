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
            var story = allRepository.AdminStory.GetAllStory();

            return View(story);
        }


        public IActionResult ApproveAndDecline(long id, int flag, string title)
        {
            if (id != 0)
            {
                if (flag == 0)
                {
                    var record = allRepository.AdminStory.GetStoryById(id);
                    record.Status = "DECLINED";
                    record.Title = title;
                    allRepository.AdminStory.DeclineStory(record);

                }
                if (flag == 1)
                {
                    var record = allRepository.AdminStory.GetStoryById(id);
                    record.Status = "PUBLISHED";
                    record.Title = title;
                    allRepository.AdminStory.DeclineStory(record);

                }
            }
            //if (id == 0)
            //{
            //    CI.Models.MissionTheme missiontheme = new CI.Models.MissionTheme()
            //    {
            //        Title = title,
            //        Status = 1,
            //        CreatedAt = DateTime.Now,
            //    };
            //    allRepository.AdminMission.AddTheme(missiontheme);
            //}
            return RedirectToAction("Story", "Story");
        }

        public IActionResult DeleteStory(long id)
        {
            if (id != 0)
            {
                    var record = allRepository.AdminStory.GetStoryById(id);
                    record.Status = "DELETE";
                record.DeletedAt=DateTime.Now;
                    allRepository.AdminStory.DeleteStory(record);

                
            }
            return RedirectToAction("Story", "Story");
        }

        public IActionResult Banner()
        {
            return View();
        }
    }
}
