using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI.Repository.Repository.IRepository
{
    public interface IAdminStory
    {
      
        IEnumerable<Models.Story> SearchStory(string searchString);
        IEnumerable<Models.Story> GetStory();

        Models.Story GetStoryById(long id);
        bool DeclineStory(Models.Story story);
        bool DeleteStory(Models.Story story);



    }
}
