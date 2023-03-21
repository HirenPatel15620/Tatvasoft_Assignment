using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CI.Models;
namespace CI.DataAcess.Repository.IRepository
{
    public interface IStory:IRepository<CI.Models.Story>
    {
        List<CI.Models.Story> GetStories();
        List<CI.Models.Mission> Get_User_Missions(long user_id);
        bool AddStory(long user_id, long mission_id, string title, string published_date, string mystory, List<string> media);
    }
}
