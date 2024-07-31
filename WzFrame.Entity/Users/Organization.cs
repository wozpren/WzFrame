using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WzFrame.Entity.Attributes;

namespace WzFrame.Entity.Users
{
    [SysTable]
    [SugarTable("Organization", TableDescription = "机构表")]
    public class Organization : IdentityUser<long>, IEntityBase
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = false)]
        [AutoGenerateColumn(Ignore = true)]
        public override long Id { get; set; }

        [AutoGenerateColumn(Text = "机构名称")]
        public string Name { get; set; } = string.Empty;







    }
}
