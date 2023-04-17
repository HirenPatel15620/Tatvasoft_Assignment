using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI.Repository.Repository.IRepository
{
    public interface IAdminStory
    {
        CI.Models.ViewModels.AdminStory GetAllStory();
        Models.Story GetStoryById(long id);
        bool DeclineStory(Models.Story story);
        bool DeleteStory(Models.Story story);

    }
}
