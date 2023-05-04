using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI.Models.ViewModels
{
    public class Mission
    {
        public string? SearchString { get; set; }
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }

        public long? MissionId { get; set; }
        public int? total_missions { get; set; }
        public List<Story>? Stories { get; set; }
        public List<CI.Models.Mission>? Missions { get; set; }
        public List<Country>? Country { get; set; }
        public List<City>? Cities { get; set; }
        public List<MissionTheme>? themes { get; set; }
        public List<Skill>? skills { get; set; }
        //public bool Applied_or_not { get; set; }
  
       public List<User>? All_volunteers { get; set; }

        public int? Availableseats { get; set; }
        public List<Timesheet>? timesheet { get; set; }
        public List<GoalMission>? goal { get; set; }
     
        public List<MissionApplication>? missionApplications { get; set; }



    }
}
