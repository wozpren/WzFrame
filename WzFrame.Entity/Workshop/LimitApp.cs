using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WzFrame.Entity.Workshop
{
    [SugarTable("LimitApp")]
    public class LimitApp : EntityBase
    {
        public string Name { get; set; } = "";
        public uint Limit { get; set; }
    }
}
