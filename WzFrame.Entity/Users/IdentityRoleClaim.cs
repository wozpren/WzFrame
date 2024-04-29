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
    [SugarTable("IdentityRoleClaim", TableDescription = "角色Claim表")]
    public class IdentityRoleClaim : IdentityRoleClaim<long>
    {
        [SugarColumn(IsPrimaryKey = true)]
        public override int Id { get => base.Id; set => base.Id = value; }
    }

}
