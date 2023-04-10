using System;
using System.Collections.Generic;

namespace CI.Models;

public partial class ContactU
{
    public long ContactId { get; set; }

    public long UserId { get; set; }

    public string Email { get; set; } = null!;

    public string Subject { get; set; } = null!;

    public string Message { get; set; } = null!;
}
