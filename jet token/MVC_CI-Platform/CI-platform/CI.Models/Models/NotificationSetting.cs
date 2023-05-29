using System;
using System.Collections.Generic;

namespace CI.Models;


public partial class NotificationSetting
{
    public long NotificationSettingId { get; set; }

    public long UserId { get; set; }

    public int RecommendMission { get; set; }

    public int RecommendStory { get; set; }

    public int VolunteerHour { get; set; }

    public int VolunteerGoal { get; set; }

    public int StoryApprove { get; set; }

    public int ApplicationApprove { get; set; }

    public int NewMission { get; set; }

    public int NewMessage { get; set; }

    public int News { get; set; }

    public int FromMail { get; set; }

    public DateTime? LastSeen { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public DateTime? UpdateAt { get; set; }
}
