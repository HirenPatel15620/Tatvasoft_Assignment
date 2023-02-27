using System;
using System.Collections.Generic;

namespace CI_platform.Models;

public partial class Skill
{
    public long SkillId { get; set; }

    public string? SkillName { get; set; }

    public int Status { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual ICollection<MissionSkill> MissionSkills { get; } = new List<MissionSkill>();

    public virtual ICollection<UserSkill> UserSkills { get; } = new List<UserSkill>();
}
