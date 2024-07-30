using BootstrapBlazor.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WzFrame.Entity.Users
{
    public partial class ApplicationUser
    {
        [AutoGenerateColumn(Text = "Í«≥∆")]
        [SugarColumn(ColumnDescription = "”√ªßÍ«≥∆", Length = 60)]
        public string DisplayName { get; set; } = string.Empty;

        public bool IsVip { get; set; }

    }
}
