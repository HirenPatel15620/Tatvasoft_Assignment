﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CI.Repository.Repository.IRepository;
using CI.Models;

namespace CI.Repository.Repository
{
    public class ResetPassword:Repository<CI.Models.PasswordReset>,IResetPassword
    {

        private readonly CiPlatformContext _db;
        public ResetPassword(CiPlatformContext db) : base(db)
        {
            _db = db;
        }

        public void DeleteData(CI.Models.PasswordReset model)
        {
            _db.PasswordResets.Remove(model);
        }
    }
}
