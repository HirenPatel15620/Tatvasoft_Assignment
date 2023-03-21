using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI.Models.ViewModels
{
    public class Mission
    {
        public int? total_missions { get; set; }
        public List<CI.Models.Mission>? Missions { get; set; }
        public List<Country>? Country { get; set; }
        public List<City>? Cities { get; set; }
        public List<MissionTheme>? themes { get; set; }
        public List<Skill>? skills { get; set; }

        public double? Avg_ratings { get; set; }
        public List<FavoriteMission> Favorite_mission { get; set; }
        public bool Applied_or_not { get; set; }

       
    }
}
