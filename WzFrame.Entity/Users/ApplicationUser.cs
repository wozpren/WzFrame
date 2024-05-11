using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using WzFrame.Entity.Attributes;
using WzFrame.Entity.Consts;

namespace WzFrame.Entity.Users
{
    public enum UserStatus
    {
        Enable = 0,
        Disabled = 1,
    }

    public enum UserGender
    {
        NoHuman = 0,
        Male = 1,
        Female = 2,
    }

    [SysTable]
    [SugarTable("User", TableDescription = "用户表")]
    public partial class ApplicationUser : IdentityUser<long>, IEntityBase
    {
        [AutoGenerateColumn(Ignore = true)]
        [SugarColumn(IsPrimaryKey = true)]
        public override long Id { get => base.Id; set => base.Id = value; }

        [AutoGenerateColumn(Text = "用户名")]
        [SugarColumn(ColumnDescription = "用户姓名", Length = 60)]
        public override string? UserName { get => base.UserName; set => base.UserName = value; }

        [SugarColumn(ColumnDescription = "格式化用户名", Length = 60)]
        [AutoGenerateColumn(Ignore = true)]
        public override string? NormalizedUserName { get => base.NormalizedUserName; set => base.NormalizedUserName = value; }

        [AutoGenerateColumn(Text = "邮箱")]
        public override string? Email { get => base.Email; set => base.Email = value; }

        [AutoGenerateColumn(Ignore = true)]
        public override string? NormalizedEmail { get => base.NormalizedEmail; set => base.NormalizedEmail = value; }

        [AutoGenerateColumn(Text = "邮箱已验证")]
        public override bool EmailConfirmed { get => base.EmailConfirmed; set => base.EmailConfirmed = value; }

        [AutoGenerateColumn(Text = "二步验证")]
        public override bool TwoFactorEnabled { get => base.TwoFactorEnabled; set => base.TwoFactorEnabled = value; }

        [AutoGenerateColumn(Text = "电话号码")]
        public override string? PhoneNumber { get => base.PhoneNumber; set => base.PhoneNumber = value; }

        [AutoGenerateColumn(Text = "电话号码已验证")]
        public override bool PhoneNumberConfirmed { get => base.PhoneNumberConfirmed; set => base.PhoneNumberConfirmed = value; }

        [AutoGenerateColumn(Text = "启用")]
        public override bool LockoutEnabled { get => base.LockoutEnabled; set => base.LockoutEnabled = value; }

        [AutoGenerateColumn(Text = "解锁时间", IsVisibleWhenEdit = false)]
        public override DateTimeOffset? LockoutEnd { get => base.LockoutEnd; set => base.LockoutEnd = value; }


        [AutoGenerateColumn(Ignore = true)]
        public override string? PasswordHash { get => base.PasswordHash; set => base.PasswordHash = value; }


        [AutoGenerateColumn(Ignore = true)]
        public override string? SecurityStamp { get => base.SecurityStamp; set => base.SecurityStamp = value; }


        [AutoGenerateColumn(Ignore = true)]
        public override int AccessFailedCount { get => base.AccessFailedCount; set => base.AccessFailedCount = value; }

        [AutoGenerateColumn(Ignore = true)]
        [Navigate(typeof(IdentityUserRole), nameof(IdentityUserRole.UserId), nameof(IdentityUserRole.RoleId))]
        public List<Role>? Roles { get; set; }

        [SugarColumn(IsIgnore = true)]
        [AutoGenerateColumn(Text = "角色", IsVisibleWhenAdd = false, IsReadonlyWhenEdit = false)]
        public string? RolesString => string.Join(",", Roles?.Select(x => x.Name));

        [SqlSugar.SugarColumn(IsEnableUpdateVersionValidation = true)]
        [AutoGenerateColumn(Ignore = true)]
        public override string? ConcurrencyStamp { get => base.ConcurrencyStamp; set => base.ConcurrencyStamp = value; }

        [AutoGenerateColumn(Text = "头像", Visible = false, IsVisibleWhenEdit = true)]
        public string? Avatar { get; set; }

        [AutoGenerateColumn(Text = "描述", Visible = false, IsVisibleWhenEdit = true)]
        public string? Description { get; set; }



    }
}
