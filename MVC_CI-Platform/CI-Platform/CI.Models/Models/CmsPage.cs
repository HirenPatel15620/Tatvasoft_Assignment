﻿using System;
using System.Collections.Generic;

namespace CI.Models.Models;

public partial class CmsPage
{
    public long CmsPageId { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public string Slug { get; set; } = null!;

    public int? Status { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public DateTime? DeletedAt { get; set; }
}