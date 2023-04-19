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
       
        User GetUserByUserId(long userid);

        bool GetUpdateUser(User user);
        IEnumerable<User> GetUsers();
        IEnumerable<User> SearchUsers(string searchString);

        //cms page///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        CmsPage GetCmsByCmsId(long cmsid);

        bool GetDeleteCms(CmsPage cmss);

        bool updatecms(CmsPage cmspage);
        CmsPage GetCmsrecordByCmsid(long id);
        bool savecms(CmsPage cmsPage);
      
        IEnumerable<CmsPage> GetCmsPages();
        IEnumerable<CmsPage> Searchcmspages(string searchString);

    }
}
