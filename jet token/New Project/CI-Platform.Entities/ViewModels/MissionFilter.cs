using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Entities.ViewModels
{
    public class MissionFilter
    {
        public List<long> CityIds { get; set; } = new List<long>();

        public List<long> CountryIds { get; set; } = new List<long>();

        public List<long> ThemeIds { get; set; } = new List<long>();

        public List<long> SkillIds { get; set; } = new List<long>();

        public string Search { get; set; } = string.Empty;

        public string SortBy { get; set; } = "CreatedAt";

        public string SortOrder { get; set; } = "Desc";

        public int PageSize { get; set; } = 3;

        public int PageNumber { get; set; } = 1;
    }
}
