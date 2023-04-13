using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace CI.Models.ViewModels
{
    public class ProfileViewModel
    {
        public List<Country>? Countries { get; set; }
        public List<City>? Cities { get; set; }
        public List<Skill>? Skills { get; set; }

        public EditUser? user { get; set; }

        public List<Admin>? AdminUser { get; set; }
        public long UserId { get; set; }
        public IFormFile? profile { get; set; }
        public string? Selected_Skills { get; set; }

        public string? EmployeeId { get; set; }

    }
}
