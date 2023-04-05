using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI.Repository.Repository.IRepository
{
    public interface IProfile
    {
        CI.Models.ViewModels.ProfileViewModel Get_Initial_Details(int country);
        bool Update_Details(CI.Models.ViewModels.ProfileViewModel Details, long User_id);
        bool Change_Password(string oldpassword, string newpassword, long User_id);

    }
}
