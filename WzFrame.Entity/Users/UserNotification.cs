using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WzFrame.Entity.DTO;

namespace WzFrame.Entity.Users
{
    public enum NotificationRange
    {
        System = 0,
        User = 1,
        Group = 2,
        All = 3
    }


    [SugarTable("UserNotification", TableDescription = "用户通知")]
    public class UserNotification : EntityBase
    {
        public long ReceiveUserId { get; set; }
        public long SendUserId { get; set; }

        [Navigate(NavigateType.OneToOne, nameof(SendUserId))]
        public UserVO? SendUser { get; set; }



        public string Type { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public bool IsRead { get; set; } = false;
        public DateTime SendTime { get; set; }



    }
}
