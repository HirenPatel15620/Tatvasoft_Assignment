using CI_Platform.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Entities.ViewModels
{
    public class MissionApplicationAdminModel
    {
        public MissionApplication missionApplication { get; set; } = new MissionApplication();

        public string UserName { get; set; } = String.Empty;

        public string MissionTitle { get; set; } = String.Empty;
    }
}
