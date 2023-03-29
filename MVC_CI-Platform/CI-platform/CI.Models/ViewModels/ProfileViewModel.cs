using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI.Models.ViewModels
{
    public class ProfileViewModel
    {

        public List<Country>? Countries { get; set; }
        public List<City>? Cities { get; set; }
        public List<Skill>? Skills { get; set; }

        public EditProfile? user { get; set; }
        public IFormFile? profile { get; set; }

        public string? Selected_Skills { get; set; }

    }
}
