using System;
using System.Collections.Generic;

namespace Model.Models
{
    public partial class User
    {
        public long UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}
