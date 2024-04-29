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
    [SugarTable("IdentityUserToken", TableDescription = "用户token表")]
    public class IdentityUserToken : IdentityUserToken<long>
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = false)]
        public override long UserId { get => base.UserId; set => base.UserId = value; }
    }
}
