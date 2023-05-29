using CI_Platform.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Entities.ViewModels
{
    public class TimeSheetModel
    {
        public Timesheet timeSheet { get; set; } = new Timesheet();

        public string MissionName { get; set;} = string.Empty;

        public int MissionType { get; set; } = 0;

        public DateTime missionStartDate { get; set; }

        public DateTime missionEndDate { get; set; }

    }
}
