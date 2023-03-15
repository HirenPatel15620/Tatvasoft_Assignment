using CI.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI.Models.ViewModels
{
    public class Volunteer_Mission
    {
        public CI.Models.Models.Mission? mission { get; set; }
        public List<CI.Models.Models.Mission>? related_mission { get; set; }
        public List<User>? Recent_volunteers { get; set; }
        public List<User>? All_volunteers { get; set; }
        public int? Total_volunteers { get; set; }
        public int? Favorite_mission { get; set; }
        public int? Rating { get; set; }
    }
}
