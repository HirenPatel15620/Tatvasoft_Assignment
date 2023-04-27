using CI.Models;
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



        public IActionResult Story(string searchString, int page)
        {
            if (HttpContext.Session.GetString("role") is null)
            {
                return RedirectToAction("Home", "Home");

            }
            int pageSize = 10; // Number of records to display per page

            IEnumerable<CI.Models.Story> stories;

            if (!string.IsNullOrEmpty(searchString))
            {
                stories = allRepository.AdminStory.SearchStory(searchString);
            }
            else
            {
                stories = allRepository.AdminStory.GetStory();
            }

            int totalRecords = stories.Count();
            int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
            int skip = (page - 1) * pageSize;

            stories = stories.Skip(skip).Take(pageSize).ToList();

            ViewData["SearchString"] = searchString;
            ViewData["CurrentPage"] = page;

            ViewData["TotalPages"] = totalPages;

            return View(stories);
            //var story = allRepository.AdminStory.GetAllStory();

            //return View(story);
        }


        public IActionResult ApproveAndDecline(long id, int flag, string title)
        {
            if (HttpContext.Session.GetString("role") is null)
            {
                return RedirectToAction("Home", "Home");

            }
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
            if (HttpContext.Session.GetString("role") is null)
            {
                return RedirectToAction("Home", "Home");

            }
            if (id != 0)
            {
                var record = allRepository.AdminStory.GetStoryById(id);
                record.Status = "DELETE";
                record.DeletedAt = DateTime.Now;
                allRepository.AdminStory.DeleteStory(record);


            }
            return RedirectToAction("Story", "Story");
        }

        public IActionResult Banner(string searchString, int? pageNumber)
        {
            if (HttpContext.Session.GetString("role") is null)
            {
                return RedirectToAction("Home", "Home");

            }
            const int pageSize = 3;

            // Get all banners
            CI.Models.ViewModels.Banner viewModel = allRepository.AdminStory.GetAllBanners();

            // Search for banners if search parameter is provided
            if (!string.IsNullOrEmpty(searchString))
            {
                viewModel.BannerList = viewModel.BannerList.Where(b => b.Text.ToLower().Contains(searchString)).ToList();
            }

            // Paginate the banners using the requested page number and page size
            viewModel.TotalPages = (int)Math.Ceiling(viewModel.BannerList.Count / (double)pageSize);
            viewModel.PageNumber = pageNumber ?? 1;
            viewModel.BannerList = viewModel.BannerList.Skip((viewModel.PageNumber - 1) * pageSize).Take(pageSize).ToList();

            viewModel.SearchString = searchString;

            return View(viewModel);
        }


       
        public async Task<IActionResult> EditBanner(int id, string Text, int SortOrder, List<IFormFile> files)
        {
            if (HttpContext.Session.GetString("role") is null)
            {
                return RedirectToAction("Home", "Home");

            }

            var record = allRepository.AdminStory.GetBannerById(id);
            if (record != null)
            {

                foreach (var file in files)
                {
                    if (file.Length > 0)
                    {
                        using var stream = new MemoryStream();
                        await file.CopyToAsync(stream);
                        record.BannerId = id;
                        record.Image = "data:image/png;base64," + Convert.ToBase64String(stream.ToArray());


                    }
                }
                record.BannerId = id;
                record.Text = Text;
                record.SortOrder = SortOrder;
                allRepository.AdminStory.editbanner(record);
            }
            return RedirectToAction("Banner", "Story");

        }

        public IActionResult DeleteBanner(int id)
        {
            if (HttpContext.Session.GetString("role") is null)
            {
                return RedirectToAction("Home", "Home");

            }
            if (id != 0)
            {
                var record = allRepository.AdminStory.GetBannerById(id);
                allRepository.AdminStory.DeleteBanner(record);

            }
            return RedirectToAction("Banner", "Story");
        }




        public async Task<IActionResult> AddBanner(CI.Models.ViewModels.Banner model, List<IFormFile> files)
        {
            if (HttpContext.Session.GetString("role") is null)
            {
                return RedirectToAction("Home", "Home");

            }
            if (model.banner.Text is not null)
            {
                foreach (var file in files)
                {
                    if (file.Length > 0)
                    {
                        using var stream = new MemoryStream();
                        await file.CopyToAsync(stream);

                        Banner banner = model.banner;
                        {
                            //  banner.BannerId = model.BannerId;
                            banner.Text = model.banner.Text;
                            banner.SortOrder = model.banner.SortOrder;
                            banner.Image = "data:image/png;base64," + Convert.ToBase64String(stream.ToArray());
                        }

                        allRepository.AdminStory.AddBanner(banner);
                    }
                }

                return RedirectToAction("Banner", "Story");

            }
            else
            {
                return RedirectToAction("Banner", "Story");

            }
        }


    }
}
