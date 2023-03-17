using CI.DataAcess.Repository.IRepository;
using CI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI.DataAcess.Repository
{
    public class UserAuthentication : Repository<User>, IUserAuthentication
    {
        private readonly CiPlatformContext _db;
        public UserAuthentication(CiPlatformContext db) : base(db)
        {
            _db = db;
        }

        public User ResetPassword(string password,string email)
        {
            User user = _db.Users.FirstOrDefault(c=>c.Email.Equals(email));
            if (user == null)
            {
                return null;
            }
            else
            {
                user.Password = password;
                user.UpdatedAt = DateTime.Now;
                return user;
            }
        }
    }
}
