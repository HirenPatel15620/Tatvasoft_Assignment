using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI.Models.ViewModels
{
    public class Banner
    {

        public List<Models.Banner> BannerList { get; set; }
        public CI.Models.Banner banner { get; set; }
        public string SearchString { get; set; }
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
    }
}
