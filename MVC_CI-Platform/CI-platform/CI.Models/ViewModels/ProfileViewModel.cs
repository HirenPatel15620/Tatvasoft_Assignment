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
    }
}
