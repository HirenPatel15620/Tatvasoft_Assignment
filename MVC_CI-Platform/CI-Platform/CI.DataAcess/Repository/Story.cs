using CI.DataAcess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CI.Models;

namespace CI.DataAcess.Repository
{
    public class Story:Repository<CI.Models.Story>,IStory
    {
        private readonly CiPlatformContext _db;
        List<MissionApplication> missionApplications = new List<MissionApplication>();
        List<CI.Models.StoryMedia> medias = new List<CI.Models.StoryMedia>();
        public Story(CiPlatformContext db) : base(db)
        {
            _db = db;
            getdetails();
        }

        public bool AddStory(long user_id, long mission_id, string title, string published_date, string mystory, List<string> media)
        {
            CI.Models.Story story = new CI.Models.Story
            {
                UserId = user_id,
                MissionId = mission_id,
                Title = title,
                Description = mystory,
                PublishedAt = DateTime.Parse(published_date),
            };
            _db.Stories.Add(story);
            _db.SaveChanges();
            long story_id = story.StoryId;
            foreach(var item in media)
            {
                _db.StoryMedia.Add(new StoryMedia
                {
                    StoryId = story_id,
                    Type="images",
                    Path = item
                }) ;
            }
            _db.SaveChanges();
            return true;
            
        }

        public void getdetails()
        {
            missionApplications = _db.MissionApplications.ToList();
            medias = _db.StoryMedia.ToList();
        }

        public List<Models.Story> GetStories()
        {
            List<Models.Story> stories = _db.Stories.ToList();
            return stories;
        }

        public List<CI.Models.Mission> Get_User_Missions(long user_id)
        {
            List<CI.Models.Mission> User_Missions = (from m in missionApplications
                                                     where m.UserId == user_id
                                                     select m.Mission).ToList();
            return User_Missions;
        }
    }
}
