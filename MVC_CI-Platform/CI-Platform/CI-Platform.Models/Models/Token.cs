using System;
using System.Collections.Generic;

namespace CI_Platform.Models.Models;

public partial class Token
{
    public long Id { get; set; }

    public string Email { get; set; } = null!;

    public string Token1 { get; set; } = null!;

    public DateTime GeneratedAt { get; set; }

    public int? Used { get; set; }
}
