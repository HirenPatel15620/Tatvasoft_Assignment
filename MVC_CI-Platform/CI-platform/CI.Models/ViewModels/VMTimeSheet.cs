using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI.Models.ViewModels
{
    public class VMTimeSheet
    {
        //public List<Mission>? Missions { get; set; }
        //public List<Timesheet>? Timesheets { get; set; }
        //public long? Id { get; set; }
        //public long? missionId { get; set; }
        //public DateOnly dateVolunteer { get; set; }
        //public int? Hour { get; set; }
        //public int? Minute { get; set; }
        //public int? Action { get; set; }
        //public string? Message { get; set; }




        public long TimesheetId { get; set; }
        public int? hour { get; set; }
        public int? minute { get; set; }
        public Timesheet timesheet { get; set; }
        public List<Timesheet> timesheets { get; set; }
        public List<MissionApplication> missionApplicatoinsByTime { get; set; }
        public List<MissionApplication> missionApplicatoinsByGoal { get; set; }







    }
}
