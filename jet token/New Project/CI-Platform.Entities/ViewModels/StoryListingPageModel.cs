using CI_Platform.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Entities.ViewModels
{
    public class StoryListingPageModel
    {
        public Story story { get; set; }

        public List<StoryMedium> storyMedium { get; set; } = new List<StoryMedium>();

        public string Username { get; set; } = string.Empty;

        public string Avtar { get; set; } = String.Empty;

        public string whyIvol { get; set; } = String.Empty ;

        public string ThemeName { get; set; } = String.Empty;

        public long MissionId { get; set; } = 0;
    }
}
