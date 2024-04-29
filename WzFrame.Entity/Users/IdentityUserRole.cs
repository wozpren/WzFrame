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
    [SugarTable("IdentityUserRole", TableDescription = "用户角色关联表")]
    public class IdentityUserRole : IdentityUserRole<long>
    {
        [SugarColumn(IsPrimaryKey = true)]
        public override long UserId { get => base.UserId; set => base.UserId = value; }

        [SugarColumn(IsPrimaryKey = true)]
        public override long RoleId { get => base.RoleId; set => base.RoleId = value; }
    }
}
