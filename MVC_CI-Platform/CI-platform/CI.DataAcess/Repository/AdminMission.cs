using CI.Models;
using CI.Repository.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI.Repository.Repository
{
    public class AdminMission : IAdminMission
    {
        private readonly CiPlatformContext _db;
        List<CI.Models.Mission> missions;
        List<CI.Models.MissionApplication> missionapplication;
        public AdminMission(CiPlatformContext db)
        {
            _db = db;
            missions = _db.Missions.ToList();
            missionapplication = _db.MissionApplications.ToList();
        }


        public List<Models.Mission> GetAllMission()
        {
            missions = missions.ToList();
            return missions;
        }

        public List<Models.MissionApplication> GetAllMissionApplication()
        {
            missionapplication = missionapplication.ToList();
            return missionapplication;
        }

        public MissionApplication GetMissionApplicationById(long id)
        {
            return _db.MissionApplications.Where(x => x.MissionApplicationId == id).FirstOrDefault();
        }

        public bool DeclineUser(MissionApplication missionapplication)
        {
            _db.MissionApplications.Update(missionapplication);
            _db.SaveChanges();
            return true;
        }
    }
}
