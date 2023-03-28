using System;
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
        public IResetPassword ResetPassword { get; }
        public IStory Story { get; }

        public IProfile Profile { get; }
    }
}
