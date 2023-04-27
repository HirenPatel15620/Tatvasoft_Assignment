using CI.Repository.Repository.IRepository;
using CI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI.Repository.Repository
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
            PasswordReset passwordReset=_db.PasswordResets.FirstOrDefault(r => r.Email == email);
            User user = _db.Users.FirstOrDefault(c=>c.Email.Equals(email));
            if (user == null)
            {
                return null;
            }
            else
            {
                
                user.Password = password;
                user.UpdatedAt = DateTime.Now;

                if (passwordReset != null)
                {
                    _db.PasswordResets.Remove(passwordReset);
                }

                _db.SaveChanges();

                return user;
            }
        }
    }
}
