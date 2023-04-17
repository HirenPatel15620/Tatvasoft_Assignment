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

        public AdminStory(CiPlatformContext db)
        {
            _db = db;
            getAllDetails();


        }
        public void getAllDetails()
        {
            story = _db.Stories.ToList();
         

        }

        public CI.Models.ViewModels.AdminStory GetAllStory()
        {
            story = story.Where(x=>x.Status!= "PUBLISHED" ).ToList();

            var stori = new CI.Models.ViewModels.AdminStory { Stories = story};
            return stori;
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
    }
}
