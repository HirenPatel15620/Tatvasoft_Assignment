using System;
using System.Collections.Generic;

namespace CI.Models;


public partial class Notification
{
    public long NotificationId { get; set; }

    public string NotificationText { get; set; } = null!;

    public long? FromUserId { get; set; }

    public long NotificationType { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public long? MissionId { get; set; }

    public long? StoryId { get; set; }

    public virtual ICollection<UserNotification> UserNotifications { get; } = new List<UserNotification>();
}
