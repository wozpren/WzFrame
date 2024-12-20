using BootstrapBlazor.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WzFrame.Entity.System
{
    [SugarTable("HubUser")]
    public class HubUser : EntityBase
    {
        public string Name { get; set; } = string.Empty;
        public string MachineCode { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        [AutoGenerateColumn(Ignore = true)]
        public string ConnectionId { get; set; } = string.Empty;


        [AutoGenerateColumn(Text = "是否在线", Sortable = true)]
        public bool IsOnline { get; set; } = false;

        [AutoGenerateColumn(Text = "最后登录时间", Sortable = true)]
        public DateTime LastLogout { get; set; }
    }
}
