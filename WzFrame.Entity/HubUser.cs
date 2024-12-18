using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WzFrame.Entity
{
    [SugarTable("HubUser")]
    public class HubUser : EntityBase
    {
        public string Name { get; set; } = string.Empty;
        public string MachineCode { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;

        public string ConnectionId { get; set; } = string.Empty;

        public bool IsOnline { get; set; } = false;
        public DateTime LastLogout { get; set; }

    }
}
