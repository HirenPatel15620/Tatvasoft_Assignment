using CI.Models;
using CI.Models.ViewModels;
using CI.Repository.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI.Repository.Repository
{
    public class AdminStory : IAdminStory
    {
        private CiPlatformContext _db;
        List<Models.Story> story = new List<Models.Story>();
        List<Models.Banner> banner = new List<Models.Banner>();
        public AdminStory(CiPlatformContext db)
        {
            _db = db;
            getAllDetails();


        }
        public void getAllDetails()
        {
            story = _db.Stories.ToList();
            banner = _db.Banners.ToList();

        }

        public IEnumerable<Models.Story> GetStory()
        {
            return _db.Stories.Where(x => x.Status != "PUBLISHED" && x.Status != "DELETE" && x.Status != "DRAFT").ToList();
        }

        public IEnumerable<Models.Story> SearchStory(string searchString)
        {
            return _db.Stories
                .Where(u => u.Title.Contains(searchString) && u.Status != "PUBLISHED" && u.Status != "DELETE" && u.Status != "DRAFT")
                .ToList();
        }





        public Models.Story GetStoryById(long id)
        {
            return _db.Stories.Where(x => x.StoryId == id).FirstOrDefault();
        }

        public bool DeclineStory(Models.Story story)
        {
            _db.Stories.Update(story);
            _db.SaveChanges();
            return true;
        }

        public bool DeleteStory(Models.Story story)
        {
            _db.Stories.Update(story);
            _db.SaveChanges();
            return true;
        }






        //banners





        public Models.ViewModels.Banner GetAllBanners()
        {
            banner = banner.OrderBy(ci => ci.SortOrder).ToList();
            Models.Banner mybanner = new Models.Banner();

            return new Models.ViewModels.Banner { BannerList = banner, banner = mybanner };
        }

        public Models.Banner GetBannerById(long id)
        {
            return _db.Banners.Where(x => x.BannerId == id).FirstOrDefault();
        }


        public bool DeleteBanner(Models.Banner banner)
        {
            _db.Banners.Remove(banner);
            _db.SaveChanges();
            return true;
        }

        public bool AddBanner(Models.Banner banner)
        {
            _db.Banners.Add(banner);
            _db.SaveChanges();
            return true;
        }

        public bool editbanner(Models.Banner banner)
        {
            _db.Banners.Update(banner);
            _db.SaveChanges();
            return true;
        }
    }
}
