using BootstrapBlazor.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WzFrame.Entity.Interfaces;

namespace WzFrame.Entity.System
{

    public enum MenuType
    {
        Directory, Menu, Button, Url
    }

    [SugarTable("Menu", TableDescription = "菜单表")]
    public class MenuOption : SysEntity, ITree<MenuOption>
    {

        [Display(Name = "ID")]
        [SugarColumn(IsPrimaryKey = true, IsIdentity = false)]
        public override long Id { get => base.Id; set => base.Id = value; }

        [Display(Name = "父类ID")]
        [SugarColumn(ColumnDescription = "父类ID")]
        [AutoGenerateColumn(Ignore = true)]
        public long ParentId { get; set; }

        [Display(Name = "菜单类型")]
        [SugarColumn(ColumnDescription = "菜单类型")]
        public MenuType Type { get; set; }

        [Display(Name = "菜单名称")]
        [SugarColumn(ColumnDescription = "菜单名称", Length = 64)]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "菜单图标")]
        [SugarColumn(ColumnDescription = "菜单图标", Length = 64, IsNullable = true)]
        public string? Icon { get; set; }

        [Display(Name = "菜单路径")]
        [SugarColumn(ColumnDescription = "菜单路径", Length = 128, IsNullable = true)]
        public string? Path { get; set; }

        [Display(Name = "菜单排序")]
        [SugarColumn(ColumnDescription = "菜单排序")]
        public int Order { get; set; }

        [Display(Name = "程序集")]
        [SugarColumn(ColumnDescription = "程序集", Length = 128, IsNullable = true)]
        [AutoGenerateColumn(Ignore = true)]
        public string? Assembly { get; set; }

        [Display(Name = "类名")]
        [SugarColumn(ColumnDescription = "类名", Length = 128, IsNullable = true)]
        [AutoGenerateColumn(Ignore = true)]
        public string? ClassName { get; set; }

        [Display(Name = "权限标识")]
        [SugarColumn(IsIgnore = true)]
        public string Permissions
        {
            get
            {
                if(Permission != null)
                return string.Join(",", Permission);
                return string.Empty;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    Permission = value.Split(',').ToList();
                }
            }
        }


        [Display(Name = "权限标识")]
        [SugarColumn(ColumnDescription = "权限标识", Length = 64, IsNullable = true, IsJson = true)]
        [AutoGenerateColumn(Ignore = true)]
        public List<string>? Permission { get; set; }


        [Display(Name = "子菜单")]
        [SugarColumn(ColumnDescription = "子菜单", IsIgnore = true)]
        [AutoGenerateColumn(Ignore = true)]
        public List<MenuOption>? Children { get; set; }
        IEnumerable<MenuOption>? ITree<MenuOption>.Children { get => Children; set => Children = value.ToList(); }
    }
}
