using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Entities.ViewModels
{
    public class BannerAdminModel
    {
        public long BannerAddEditId { get; set; } = 0;

        public string BannerText { get; set; } = string.Empty;

        public int BannerSortOrder { get; set; } = 0;

        public IFormFile? BannerImage { get; set; }
    }
}
