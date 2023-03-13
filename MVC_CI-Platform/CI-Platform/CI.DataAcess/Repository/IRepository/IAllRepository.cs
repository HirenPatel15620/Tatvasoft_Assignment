﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI.DataAcess.Repository.IRepository
{
    public interface IAllRepository
    {
        void save();
        public IUserAuthentication UserAuthentication { get; }
        public IMission Mission { get; }
        public IMissionMedia MissionMedia { get; }
        public IResetPassword ResetPassword { get; }
    }
}
