using CI_platform.Entities.DataModels;
using CI_platform.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_platform.Repository.Repository
{
  public class UserRepository: IUserRepository
    {
        private readonly CiPlatformDbContext _CiPlatformDbContext;
        public UserRepository(CiPlatformDbContext ciPlatformDbContext)
        {
            _CiPlatformDbContext = ciPlatformDbContext; 
        }
        public List<Admin> GetAdminsList ()
        {
            List<Admin> lstadmins= _CiPlatformDbContext.Admins.ToList();
            return lstadmins;
        }
    }
}
