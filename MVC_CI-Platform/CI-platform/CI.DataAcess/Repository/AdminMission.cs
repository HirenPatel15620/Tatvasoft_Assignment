using CI.Models;
using CI.Repository.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI.Repository.Repository
{
    public class AdminMission :IAdminMission
    {
        private readonly CiPlatformContext _db;
        List<CI.Models.Mission> missions;
        public AdminMission(CiPlatformContext db) 
        {
            _db = db;
            missions = _db.Missions.ToList();
        }


        public List<Models.Mission> GetAllMission()
        {
            missions = missions.ToList();
            return missions;
        }
    }
}
