using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI.DataAcess.Repository.IRepository
{
    public interface IProfile
    {
        CI.Models.ViewModels.ProfileViewModel Get_Initial_Details(int country);
        bool Update_Profile(CI.Models.ViewModels.ProfileViewModel Details, long User_id);
    }
}
