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
    [SugarTable("IdentityUserLogin", TableDescription = "角色登录表")]
    public class IdentityUserLogin : IdentityUserLogin<long>
    {
        [SugarColumn(IsPrimaryKey = true)]
        public override long UserId { get => base.UserId; set => base.UserId = value; }
    }

}
