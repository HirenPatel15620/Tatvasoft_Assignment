using CI.Models.ViewModels;
using CI.Repository.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace CI_platform.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly IAllRepository allRepository;
        public UserController(IAllRepository _allRepository)
        {
            allRepository = _allRepository;
        }

        ///////////////////////////////////////           User                       //////////////////////////////////////////////////////////////////////////////////////////////////////////

        public IActionResult Index()
        {
            var alluser = allRepository.AdminUser.GetAllUser();
            return View(alluser);
        }



        [HttpPost]
        public IActionResult GetEditUser(long id, int flag)
        {
            if (id != 0)
            {
                if (flag == 0)
                {
                    var record = allRepository.AdminUser.GetUserByUserId(id);
                    record.Status = "0";
                    allRepository.AdminUser.GetUpdateUser(record);

                }
                if (flag == 1)
                {
                    var record = allRepository.AdminUser.GetUserByUserId(id);
                    record.Status = "1";
                    allRepository.AdminUser.GetUpdateUser(record);

                }
            }
            return RedirectToAction("Index", "User");
        }

        /////////////////////////////////////////////////////////////////////////////////   CMS Page /////////////////////////////////////////////////////////////////////////////////////////////////// 

        public IActionResult Cmspage()
        {
            var allcms = allRepository.AdminUser.GetAllCms();

            return View(allcms);
        }
        [HttpPost]
        public IActionResult Editcms(long id, string title, string description, string slug, string status)
        {
            if (id == 0)
            {
                CI.Models.CmsPage cmsPage = new CI.Models.CmsPage();
                cmsPage.Title = title;
                cmsPage.Description = description;
                cmsPage.Slug = slug;
                cmsPage.Status = status;

                allRepository.AdminUser.savecms(cmsPage);
                return Json(new { success = true });

            }
            if (id != 0)
            {

                var record = allRepository.AdminUser.GetCmsrecordByCmsid(id);
                record.Title = title;
                record.Description = description;
                record.Slug = slug;
                record.Status = "1";
                record.UpdatedAt = DateTime.Now;
                allRepository.AdminUser.updatecms(record);
                return Json(new { success = true });

            }
            return View();
        }
        [HttpPost]
        public IActionResult GetDeleteCms(long cmsid)
        {
            var record = allRepository.AdminUser.GetCmsByCmsId(cmsid);
            record.CmsPageId = cmsid;
            record.Status = "0";
            allRepository.AdminUser.GetDeleteCms(record);
            return RedirectToAction("Cmspage", "User");
        }

    }
}
