using BootstrapBlazor.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WzFrame.Entity.Configuration
{
    public class WebsiteConfig : IJsonConfiguration
    {
        public bool IsOpen { get; set; } = true;
        public bool SupportRegister { get; set; }
        public bool SupportFindPassword { get; set; }
        public string SiteName { get; set; } = "WzFrame";

        public string SiteDescription { get; set; } = "个人开发Blazor框架";
        public string DefaultPassword { get; set; } = "123456";



    }
}
