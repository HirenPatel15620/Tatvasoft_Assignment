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
        void Add_View(long user_id, long story_id);
        CI.Models.ViewModels.Mission GetStories(long user_id);
        CI.Models.ViewModels.Mission GetFileredStories(int page_index, long user_id);
        List<CI.Models.Mission> Get_User_Missions(long user_id);
        bool AddStory(long user_id,long story_id, long mission_id, string title, string published_date, string mystory, List<string> media,string type);
        CI.Models.ViewModels.StoryViewModel GetStory(long user_id, long id);

        bool Recommend(long user_id, long story_id, List<long> co_workers);

        CI.Models.ViewModels.Mission GetSearchStory(string key);
    }
}
