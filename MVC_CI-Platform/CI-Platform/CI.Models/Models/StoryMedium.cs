﻿using System;
using System.Collections.Generic;

namespace CI.Models.Models;

public partial class StoryMedium
{
    public long StoryMediaId { get; set; }

    public long? StoryId { get; set; }

    public string? Type { get; set; }

    public string? Path { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual Story? Story { get; set; }
}