using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CI.Models;

public partial class Banner
{
    public long BannerId { get; set; }

    public string Image { get; set; } = null!;
    [Required(ErrorMessage = "Text can't be null")]

    public string? Text { get; set; }
    [Required(ErrorMessage = "SortOrder can't be null")]

    public int? SortOrder { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }
}
