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
    }
}
