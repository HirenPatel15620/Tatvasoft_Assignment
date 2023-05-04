using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI.Models.ViewModels
{
    public partial class ResetPassword
    {
        [Required]
        [MinLength(8,ErrorMessage ="enter valid password")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Confirm Password Not Match!")]
        public string ConfirmPassword { get; set; } = null!;
        [Required]
        public string? Token { get; set; }

        public string? Email { get; set; }
    }
}
