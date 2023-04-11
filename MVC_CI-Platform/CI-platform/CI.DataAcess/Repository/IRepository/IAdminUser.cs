using CI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI.Repository.Repository.IRepository
{
    public interface IAdminUser
    {
        List<Models.User> GetAllUser();

        bool GetDeleteUser(User user);

        User GetUserByUserId(long userid);

        List<Models.CmsPage> GetAllCms();

        CmsPage GetCmsByCmsId(long cmsid);

        bool GetDeleteCms(CmsPage cmss);
       


    }
}
