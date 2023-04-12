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
        //user page///////////////////////////////////////////////////////////////////
        List<Models.User> GetAllUser();
        User GetUserByUserId(long userid);

        bool GetDeleteUser(User user);

        //cms page///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        List<Models.CmsPage> GetAllCms();

        CmsPage GetCmsByCmsId(long cmsid);

        bool GetDeleteCms(CmsPage cmss);

        bool updatecms(CmsPage cmspage);
        CmsPage GetCmsrecordByCmsid(long id);
        bool savecms(CmsPage cmsPage);


    }
}
