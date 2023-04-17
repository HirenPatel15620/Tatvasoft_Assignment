﻿using CI.Models;
using CI.Repository.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI.Repository.Repository
{
    public class AdminUser : Repository<CI.Models.User>,IAdminUser
    {

        private readonly CiPlatformContext _db;
        List<CI.Models.User> users = new List<User>();
        List<CI.Models.CmsPage> cmss=new List<CmsPage>();
        List<CI.Models.Mission> missions;
        public AdminUser(CiPlatformContext db) : base(db)
        {
            _db = db;
            users = _db.Users.ToList();
            cmss=_db.CmsPages.ToList();
            missions = _db.Missions.ToList();

        }

        /// //////////////////////////////////////////////////////////////  User ////////////////////////////////////////////////////////

        public List<Models.User> GetAllUser()
        {
            users=users.ToList();
            return users;
        }


        public User GetUserByUserId(long userid)
        {
            return _db.Users.Where(x => x.UserId == userid).FirstOrDefault();
        }

        public bool GetUpdateUser(User user)
        {
            _db.Users.Update(user);
            _db.SaveChanges();
            return true;
        }




        ///////////////////////////////////////////////////////    cms page     /////////////////////////////////////////////////////////////////

        public List<Models.CmsPage> GetAllCms()
        {
            cmss = cmss.ToList();
            return cmss;
        }
        public CmsPage GetCmsByCmsId(long cmsid)
        {
            return _db.CmsPages.Where(x => x.CmsPageId == cmsid).FirstOrDefault();
        }

        

        public CmsPage GetCmsrecordByCmsid(long cmsid)
        {
            return _db.CmsPages.Where(x => x.CmsPageId == cmsid).FirstOrDefault();
        }

        public bool savecms(CmsPage cmsPage)
        {
            _db.CmsPages.Add(cmsPage);
            _db.SaveChanges();
            return true;
        }

        public bool GetDeleteCms(CmsPage cmss)
        {
            _db.CmsPages.Update(cmss);
            _db.SaveChanges();
            return true;
        }


        public bool updatecms(CmsPage cmspage)
        {
            _db.CmsPages.Update(cmspage);
            _db.SaveChanges();
            return true;
        }

        ////////////////////////// //////////////////////////////////////////////




    }
}