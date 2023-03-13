using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace CI.DataAcess.Repository.IRepository
{
    public interface IMission:IRepository<CI.Models.Models.Mission>
    {
        void Save();
        List<Models.ViewModels.Mission> GetAllMission();
        List<Models.ViewModels.Mission> GetFilteredMissions(List<string> countries,List<string>cities, List<string> themes, List<string> skills,string sort_by);
        List<Models.ViewModels.Mission> GetSearchMissions(string key);


    }
}
