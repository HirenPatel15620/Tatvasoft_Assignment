using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI.Models.ViewModels
{
    public class StoryViewModel
    {
        public CI.Models.Story? story { get; set; }
        public List<User>? co_workers { get; set; }

      
        public int? total_story { get; set; }

    }
}
