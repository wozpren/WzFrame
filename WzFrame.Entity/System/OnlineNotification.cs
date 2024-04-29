using BootstrapBlazor.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WzFrame.Entity.System
{
    public class OnlineNotification
    {
        public string? Title { get; set; }
        public string? Content { get; set; }

        public ToastCategory Category { get; set; } = ToastCategory.Success;

    }
}
