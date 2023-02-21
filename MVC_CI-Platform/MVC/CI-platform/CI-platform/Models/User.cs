using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace CI_platform.Models;

public partial class User
{
    [Key]
    public long UserId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }
    [Required]
    [RegularExpression(@"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z",
        ErrorMessage = "Please enter correct email address")]
    public string Email { get; set; } = null!;
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;
    [Required(ErrorMessage = "PhoneNumber is Required")]
    [MaxLength(12,ErrorMessage ="Enter Valid Number")]
    [RegularExpression(@"^\(?([0-9]{5})\)?[-. ]?([0-9]{5})[-. ]?([0-9]{2})$",
                   ErrorMessage = "Entered phone-number format is not valid.")]
    public string PhoneNumber { get; set; } = null!;

    public string? Avatar { get; set; }

    public string? WhyIVolunteer { get; set; }

    public string? EmployeeId { get; set; }

    public string? Department { get; set; }

    public long? CityId { get; set; }

    public long? CountryId { get; set; }

    public string? ProfileText { get; set; }

    public string? LinkedInUrl { get; set; }

    public string? Title { get; set; }

    public int Status { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual City? City { get; set; }

    public virtual Country? Country { get; set; }
}
