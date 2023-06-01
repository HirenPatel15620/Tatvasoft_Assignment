using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface
{
    public interface IUser
    {
        Task<List<Model.Models.User>> GetAllUsers();
        Task<Model.Models.User> GetUserById(long id);
        Task<Model.Models.User> AddUser(Model.Models.User user);
        Task<Model.Models.User> UpdateUser(Model.Models.User user);
        Task<bool> DeleteUser(long id);

    }
}
