using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace CI.Models.ViewModels;

public partial class Register
{
    [Required]
    public string? FirstName { get; set; }

    public string? LastName { get; set; }
    [Required]
    [RegularExpression(@"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z",
        ErrorMessage = "Please enter correct email address in small word")]
    public string Email { get; set; } = null!;
    [Required]
    [MinLength(8,ErrorMessage ="Enter Valid Password")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;
    [Required]
    [DataType(DataType.Password)]
    [Compare("Password",ErrorMessage ="Confirm Password Not Match!")]
    public string ConfirmPassword { get; set; }=null!;
    [Required(ErrorMessage = "PhoneNumber is Required")]
    [MaxLength(12, ErrorMessage = "Enter Valid Number")]
    [RegularExpression(@"^\(?([0-9]{5})\)?[-. ]?([0-9]{5})[-. ]?([0-9]{2})$",
                   ErrorMessage = "Entered phone-number format is not valid.")]
    public string PhoneNumber { get; set; } = null!;

}
