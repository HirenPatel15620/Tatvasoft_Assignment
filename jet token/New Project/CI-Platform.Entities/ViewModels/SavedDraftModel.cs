using CI_Platform.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Entities.ViewModels
{
    public class SavedDraftModel
    {
        public Story DraftStory { get; set; }

        public List<String>? DraftMedia { get; set; }

        public List<String>? VideoURL { get; set; }



    }
}
