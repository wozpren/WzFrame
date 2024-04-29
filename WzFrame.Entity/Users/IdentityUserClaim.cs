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
    [SugarTable("IdentityUserClaim", TableDescription = "用户Claim表")]
    public class IdentityUserClaim : IdentityUserClaim<long>
    {
        [SugarColumn(IsPrimaryKey = true)]
        public override int Id { get => base.Id; set => base.Id = value; }
    }

}
