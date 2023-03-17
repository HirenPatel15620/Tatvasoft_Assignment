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
        List<CI.Models.Mission> missions = new List<CI.Models.Mission>();
        public Story(CiPlatformContext db) : base(db)
        {
            _db = db;
            getdetails();
        }
        public void getdetails()
        {
            missionApplications = _db.MissionApplications.ToList();
            missions = _db.Missions.ToList();
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
