using System;
using System.Collections.Generic;

namespace CI.Models;

public partial class UserToken
{
    public long Id { get; set; }

    public string Email { get; set; } = null!;

    public string UserToken1 { get; set; } = null!;

    public DateTime GeneratedAt { get; set; }

    public int? Used { get; set; }
}
