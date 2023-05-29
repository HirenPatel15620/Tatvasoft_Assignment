using CI_Platform.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Entities.ViewModels
{
    public class StoryAdminModel
    {
        public Story story { get; set; } = new Story();

        public string? Username { get; set; } = string.Empty;

        public string? MissionTitle  { get; set; } = string.Empty;

        public List<StoryMedium> storyMedium { get; set; } = new List<StoryMedium>();
    }
}
