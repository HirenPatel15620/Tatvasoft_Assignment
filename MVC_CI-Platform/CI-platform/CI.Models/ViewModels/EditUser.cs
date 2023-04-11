using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI.Models.ViewModels
{
    public class EditUser
    {
        [Required]
        [MinLength(3, ErrorMessage = "minimum write 4 letters")]
        public string? FirstName { get; set; }
        [Required]
        [MinLength(3, ErrorMessage = "minimum write 4 letters")]
        public string? LastName { get; set; }

        public string? Avatar { get; set; }

        public string? WhyIVolunteer { get; set; }

        public string? EmployeeId { get; set; }

        public string? Department { get; set; }
        public string? Availablity { get; set; }
        [Required]
        public long? CityId { get; set; }
        [Required]
        public long? CountryId { get; set; }
        [Required]
        [MinLength(20, ErrorMessage = "Bio Is Too Short")]
        public string? ProfileText { get; set; }

        public string? LinkedInUrl { get; set; }

        public string? Title { get; set; }

        public long UserId { get; set; }
        public string? Manager { get; set; }

        public string? Email { get; set; }
    }
}
