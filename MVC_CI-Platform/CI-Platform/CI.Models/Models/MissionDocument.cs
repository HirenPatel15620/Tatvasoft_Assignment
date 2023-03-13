using System;
using System.Collections.Generic;

namespace CI.Models.Models;

public partial class MissionDocument
{
    public long MissionDocumentId { get; set; }

    public long MissionId { get; set; }

    public string? DocumetName { get; set; }

    public string? DocumentType { get; set; }

    public string? DocumetPath { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual Mission Mission { get; set; } = null!;
}
