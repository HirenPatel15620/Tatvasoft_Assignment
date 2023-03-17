using CI.DataAcess.Repository.IRepository;
using CI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI.DataAcess.Repository
{
    public class AllRepository : IAllRepository
    {
        private readonly CiPlatformContext _db;
        public AllRepository(CiPlatformContext db)
        {
            _db = db;
            UserAuthentication = new UserAuthentication(_db);
            Mission = new Mission(_db);
            ResetPassword = new ResetPassword(_db);
            Story=new Story(_db);
        }
        public IUserAuthentication UserAuthentication { get;private set; }

        public IMission Mission { get; private set; }

        public IResetPassword ResetPassword { get; private set; }
        public IStory Story { get; private set; }

        public void save()
        {
            _db.SaveChanges();
        }
    }
}
