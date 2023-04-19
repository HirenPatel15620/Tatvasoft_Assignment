using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI.Models.ViewModels
{
    public class AdminStory
    {

        public List<Story> Stories { get; set; }=new List<Story>();
        public Story   story { get; set; }  = new Story();



    }
}
