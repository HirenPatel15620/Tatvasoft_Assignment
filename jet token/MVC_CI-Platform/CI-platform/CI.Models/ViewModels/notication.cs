using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI.Models.ViewModels
{
    public class notication
    {
        public DateTime LastSeen { get; set; }
        public Models.NotificationSetting NotificationSettings { get; set; }
        public List<UserNotification> OldUserNotifications { get; set; }
        public List<UserNotification> NewUserNotifications { get; set; }
        public List<User> Users { get; set; }


    }
}
