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

        public IActionResult Index(string searchString, int page)
        {
            if (HttpContext.Session.GetString("role") is null)
            {
                return RedirectToAction("Home", "Home");

            }
        

            int pageSize = 10; // Number of records to display per page

            IEnumerable<CI.Models.User> users;

            if (!string.IsNullOrEmpty(searchString))
            {
                users = allRepository.AdminUser.SearchUsers(searchString);
            }
            else
            {
                users = allRepository.AdminUser.GetUsers();
            }

            int totalRecords = users.Count();
            int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
            int skip = (page - 1) * pageSize;

            users = users.Skip(skip).Take(pageSize).ToList();

            ViewData["SearchString"] = searchString;
            ViewData["CurrentPage"] = page;

            ViewData["TotalPages"] = totalPages;

            return View(users);
        }



        [HttpPost]
        public IActionResult GetEditUser(long id, int flag, string firstname, string lastname, string email, string employeeid, string department, string profiletext,string role,string password,string Phone)
        {
            if (HttpContext.Session.GetString("role") is null)
            {
                return RedirectToAction("Home", "Home");

            }
            if (id != 0)
            {
                
                    var record = allRepository.AdminUser.GetUserByUserId(id);
                    record.Status = flag.ToString();
                    record.FirstName = firstname;
                    record.LastName = lastname;
                    record.Email = email;
                    record.Department = department;
                    record.ProfileText = profiletext;
                    record.EmployeeId = employeeid;
                    record.Role = role;

                    allRepository.AdminUser.GetUpdateUser(record);
                
            }

            if (id == 0)
            {
                    string secpass = BCrypt.Net.BCrypt.HashPassword(password);
                CI.Models.User user = new CI.Models.User();
                {
                    user.FirstName = firstname;
                    user.LastName = lastname;
                    user.Email = email;
                    user.Department = department;
                    user.ProfileText = profiletext;
                    user.EmployeeId = employeeid;
                    user.PhoneNumber = Phone;
                    user.Role = role;
                    user.Password=secpass;
                    user.Status = flag.ToString();
                }
                allRepository.AdminUser.AddUser(user);

            }
            return RedirectToAction("Index", "User");
        }

        /////////////////////////////////////////////////////////////////////////////////   CMS Page /////////////////////////////////////////////////////////////////////////////////////////////////// 

        public IActionResult Cmspage(string searchString, int page)
        {
            if (HttpContext.Session.GetString("role") is null)
            {
                return RedirectToAction("Home", "Home");

            }


            int pageSize = 10; // Number of records to display per page

            IEnumerable<CI.Models.CmsPage> cmsPages;

            if (!string.IsNullOrEmpty(searchString))
            {
                cmsPages = allRepository.AdminUser.Searchcmspages(searchString);
            }
            else
            {
                cmsPages = allRepository.AdminUser.GetCmsPages();
            }

            int totalRecords = cmsPages.Count();
            int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
            int skip = (page - 1) * pageSize;

            cmsPages = cmsPages.Skip(skip).Take(pageSize).ToList();

            ViewData["SearchString"] = searchString;
            ViewData["CurrentPage"] = page;

            ViewData["TotalPages"] = totalPages;

            return View(cmsPages);



        }
        [HttpPost]
        public IActionResult Editcms(long id, string title, string description, string slug, string status)
        {
            if (HttpContext.Session.GetString("role") is null)
            {
                return RedirectToAction("Home", "Home");

            }
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
                record.Status = status;
                record.UpdatedAt = DateTime.Now;
                allRepository.AdminUser.updatecms(record);
                return Json(new { success = true });

            }
            return View();
        }
        [HttpPost]
        public IActionResult GetDeleteCms(long cmsid)
        {
            if (HttpContext.Session.GetString("role") is null)
            {
                return RedirectToAction("Home", "Home");

            }
            var record = allRepository.AdminUser.GetCmsByCmsId(cmsid);
            record.CmsPageId = cmsid;
            record.Status = "0";
            allRepository.AdminUser.GetDeleteCms(record);
            return RedirectToAction("Cmspage", "User");
        }






    }
}
