using CI_Platform.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Entities.ViewModels
{

    public class PageList<T> where T : class
    {
        public PageList(List<T> records, int totalcount)
        {
            Records = records ?? new List<T>();
            TotalCount = totalcount;
        }

        public List<T> Records { get; set; }
        public int TotalCount { get; set; }
    }

    public class MissionCard
    {
        public Mission mission { get; set; } = new Mission();

        public string? Theme { get; set; } 

        public string? city { get; set; }

        public int? seatsleft { get; set; }

        public List<string>? skills { get; set; } 

        public float? rating { get; set; }

        public int? volunteercount { get; set; }

        public bool? favMission { get; set; }

        public bool? applied { get; set; }  

        public List<MissionMedium>? missionMediums { get; set;}

        public float? progressBar { get; set; }

        public int? achievedProgress { get; set; }

        public int? totalGoal { get; set; }

        public string? goalObjective { get; set; }

    }
}
