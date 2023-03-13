using CI.DataAcess.Repository.IRepository;
using CI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CI.DataAcess.Repository
{
    public class MissionMedia : Repository<CI.Models.MissionMedia>,IMissionMedia
    {
        private readonly CiPlatformContext _db;
        public MissionMedia(CiPlatformContext db) : base(db)
        {
            _db = db;
        }
    }
}
