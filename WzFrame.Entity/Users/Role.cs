using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Identity;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using WzFrame.Entity.Attributes;

namespace WzFrame.Entity.Users
{
    [SysTable]
    [SugarTable("Role", TableDescription = "角色表")]
    public class Role : IdentityRole<long>, IEntityBase
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = false)]
        public override long Id { get => base.Id; set => base.Id = value; }

        [AutoGenerateColumn(Text = "显示名称")]
        public string? DisplayName { get; set; }

        [AutoGenerateColumn(Text = "角色名称")]
        public override string? Name { get => base.Name; set => base.Name = value; }

        [AutoGenerateColumn(Ignore = true)]
        public override string? NormalizedName { get => base.NormalizedName; set => base.NormalizedName = value; }

        [AutoGenerateColumn(Ignore = true)]
        public override string? ConcurrencyStamp { get => base.ConcurrencyStamp; set => base.ConcurrencyStamp = value; }

        [Navigate(typeof(IdentityUserRole), nameof(IdentityUserRole.RoleId), nameof(IdentityUserRole.UserId))]
        [AutoGenerateColumn(Ignore = true)]
        public List<ApplicationUser>? Users { get; set; }

    }
}
