using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CI.Models;
namespace CI.DataAcess.Repository.IRepository
{
    public interface IUserAuthentication:IRepository<User>
    {
     User ResetPassword(string password,string email); 
    }
}
