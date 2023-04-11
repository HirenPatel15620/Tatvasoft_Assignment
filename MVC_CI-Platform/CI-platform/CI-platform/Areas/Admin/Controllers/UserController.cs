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

        public IActionResult GetEditUser()
        {
            return View();
        }

        public IActionResult GetDeleteUser(int userid)
        {
            var record = allRepository.AdminUser.GetUserByUserId(userid);
            allRepository.AdminUser.GetDeleteUser(record);
            return RedirectToAction("Index", "User");
        }



        /////////////////////////////////////////////////////////////////////////////////   CMS Page /////////////////////////////////////////////////////////////////////////////////////////////////// 

        public IActionResult Cmspage()
        {
            var allcms = allRepository.AdminUser.GetAllCms();

            return View(allcms);
        }

        public IActionResult GetDeleteCms(long cmsid)
        {
            var record = allRepository.AdminUser.GetCmsByCmsId(cmsid);
            allRepository.AdminUser.GetDeleteCms(record);
            return RedirectToAction("Cmspage", "User");
        }

    }
}
