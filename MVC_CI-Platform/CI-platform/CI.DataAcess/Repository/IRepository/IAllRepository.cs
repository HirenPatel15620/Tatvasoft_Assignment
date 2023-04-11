using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI.Repository.Repository.IRepository
{
    public interface IAllRepository
    {
        void save();
        public IUserAuthentication UserAuthentication { get; }
        public IMission Mission { get; }
        public IResetPassword ResetPassword { get; }
        public IStory Story { get; }

        public IProfile Profile { get; }

        public ISheet Sheet { get; }

        public IAdminUser AdminUser { get; }

        public IAdminMission AdminMission { get; }
    }
}
