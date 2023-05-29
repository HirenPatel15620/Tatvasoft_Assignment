using CI_Platform.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CI_Platform.Entities.ViewModels
{
    public class VolunteeringTimesheetModel
    {
        public List<TimeSheetModel> timesheetModel { get; set; } = new List<TimeSheetModel> ();

        public List<SelectListItem> MissionList { get; set; } = new List<SelectListItem> ();

       
    }
}
