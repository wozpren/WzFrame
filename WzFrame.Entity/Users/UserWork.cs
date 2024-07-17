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
        [AutoGenerateColumn(Text = "�ǳ�")]
        [SugarColumn(ColumnDescription = "�û��ǳ�", Length = 60)]
        public string DisplayName { get; set; } = string.Empty;

        public bool IsVip { get; set; }

    }
}
