using CI.Models.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI.Models.ViewModels
{
    public class AdminMission
    {

        //public List<MissionMedia> MissionMedias { get; set; }




        //public string SearchString { get; set; }
        //public PaginatedList<User> Users { get; set; }


        public long MissionId { get; set; }
        public string? MissionType { get; set; }
        public string? Availability { get; set; }
        public List<CI.Models.Mission>? Missions { get; set; }
        public List<Country>? Country { get; set; }
        public List<City>? Cities { get; set; }
        public List<MissionTheme>? themes { get; set; }
        public List<Skill>? skills { get; set; }
        //public bool Applied_or_not { get; set; }

        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public string? OrganizationName { get; set; }

        public string? OrganizationDetail { get; set; }
        public int TotalSeats { get; set; }
        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
        public DateTime Deadline { get; set; }


        public long CityId { get; set; }

        public long CountryId { get; set; }
        public long MissionThemeId { get; set; }
        public int SkillId { get; set; }

    }
}
