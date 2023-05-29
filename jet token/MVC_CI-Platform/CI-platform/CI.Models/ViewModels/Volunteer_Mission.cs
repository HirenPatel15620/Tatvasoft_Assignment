using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI.Models.ViewModels
{
    public class Volunteer_Mission
    {
        public CI.Models.Mission? mission { get; set; }
        public List<CI.Models.Mission>? related_mission { get; set; }
        public List<User>? Recent_volunteers { get; set; }
        public List<User>? All_volunteers { get; set; }
        public int? Total_volunteers { get; set; }
        public int? Favorite_mission { get; set; }
        public int? Rating { get; set; }

        public double? Avg_ratings { get; set; }
        public int? Rating_count { get; set; }
        public bool Applied_or_not { get; set; }

        public List<GoalMission>? goal { get; set; }
        public List<Timesheet>? timesheet { get; set; }

        public List<MissionApplication> missionApplications { get; set; }  

        //public List<MissionDocument>? document { get; set;}
    }
}
