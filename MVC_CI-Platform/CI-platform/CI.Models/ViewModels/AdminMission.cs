using CI.Models.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI.Models.ViewModels
{
    public class AdminMission
    {

        public string? SearchString { get; set; }
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
        public long MissionId { get; set; }
        [Required(ErrorMessage = "Select mission type")]
        public string? MissionType { get; set; }
        public int GoalValue { get; set; }
     
        public string? GoalObjectiveText { get; set; } 
        [Required(ErrorMessage = "Select Availability.")]
        public string? Availability { get; set; }
        public List<CI.Models.Mission>? Missions { get; set; }
     
        public List<CI.Models.MissionMedia>? Missionmedia { get; set; }

        public List<CI.Models.MissionDocument>? missionDocuments { get; set; }
        public List<Country>? Country { get; set; }
        public List<City>? Cities { get; set; }
        public List<MissionTheme>? themes { get; set; }
        public List<Skill>? skills { get; set; }
        public List<MissionSkill>? missionSkills { get; set; }
        [Required(ErrorMessage = "Select Skill")]
        public int[]? SelectedSkills { get; set; }

        [Required]
        [MinLength(50, ErrorMessage = "Min Length 50 required")]
        public string Title { get; set; } = null!;

        [Required]
        [MinLength(100, ErrorMessage = "Min Length 100 required")]

        public string? Description { get; set; }
        [Required(ErrorMessage = "Organization Name can't be null")]
        public string? OrganizationName { get; set; }
        [Required(ErrorMessage = "Organization Details can't be null")]
        public string? OrganizationDetail { get; set; }
        [Required(ErrorMessage = "Total Seats can't be null")]
        public int TotalSeats { get; set; }
        [Required(ErrorMessage = "StartDate is required.")]
        [DataType(DataType.DateTime)]
        public DateTime? StartDate { get; set; }
        [Required(ErrorMessage = "EndDate is required.")]
        [DataType(DataType.DateTime)]
        public DateTime? EndDate { get; set; }
        [Required(ErrorMessage = "Deadline is required.")]
        [DataType(DataType.DateTime)]
        public DateTime Deadline { get; set; }
        [Required(ErrorMessage = "City can't be null")]
        public long CityId { get; set; }
        [Required(ErrorMessage = "Country can't be null")]
        public long CountryId { get; set; }
        [Required(ErrorMessage = "Theme can't be null")]
        public long? MissionThemeId { get; set; }
        public int SkillId { get; set; }

    }
}
