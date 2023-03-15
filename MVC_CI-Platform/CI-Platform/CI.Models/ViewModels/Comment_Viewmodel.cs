using CI.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI.Models.ViewModels
{
    public class Comment_Viewmodel
    {
        public Comment? User_Comment { get; set; }
        public User? user { get; set; }
       
    }
}
