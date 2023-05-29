using CI_Platform.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Entities.ViewModels
{
    public class CommentsModel
    {
        public Comment? comment { get; set; }

        public string? username { get; set; }

        public string? avtar { get; set; }

    }
}
