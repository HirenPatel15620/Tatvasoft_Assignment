using CI_Platform.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Entities.ViewModels
{
    public class VolunteerMissionPage
    {
        public MissionCard missionCard { get; set; }

        public List<MissionCard> relatedmission { get; set; }

        public List<CommentsModel> comment { get; set; }

        public List<MissionDocument> missionDocuments { get; set; }

        public List<MissionApplication>? volunteerAvtar { get; set; }

        public int? UserRating { get; set; }

        
    }
}
