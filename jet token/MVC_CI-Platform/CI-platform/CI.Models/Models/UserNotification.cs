using System;
using System.Collections.Generic;

namespace CI.Models;


public partial class UserNotification
{
    public long UserNotificationId { get; set; }

    public long NotificationId { get; set; }

    public long UserId { get; set; }

    public int? IsRead { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public virtual Notification Notification { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
