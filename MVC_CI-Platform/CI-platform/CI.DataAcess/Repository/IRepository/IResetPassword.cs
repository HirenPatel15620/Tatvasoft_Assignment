using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CI.Models;
namespace CI.DataAcess.Repository.IRepository
{
    public interface IResetPassword:IRepository<PasswordReset>
    {
        public void DeleteData(PasswordReset model);
    }
}
