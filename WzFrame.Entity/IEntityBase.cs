using BootstrapBlazor.Components;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WzFrame.Entity.Attributes;
using WzFrame.Entity.Consts;
using WzFrame.Entity.Users;
using WzFrame.Entity.DTO;

namespace WzFrame.Entity
{
    public interface IEntityBase
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = false)]
        [AutoGenerateColumn(Ignore = true)]
        public long Id { get; set; }

    }

    public abstract class EntityBase : IEntityBase
    {

        [SugarColumn(IsPrimaryKey = true, IsIdentity = false)]
        [AutoGenerateColumn(Ignore = true)]
        public virtual long Id { get; set; }
    }


    public abstract class SysEntity : IEntityBase
    {

        [SugarColumn(IsPrimaryKey = true, IsIdentity = false)]
        [AutoGenerateColumn(Ignore = true)]
        public virtual long Id { get; set; }
    }

    public abstract class EntityUserBase : IEntityBase
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = false)]
        [AutoGenerateColumn(Ignore = true)]
        public virtual long Id { get; set; }


        [AutoGenerateColumn(Text = "创建者Id", Ignore = true, IsVisibleWhenAdd = false, IsVisibleWhenEdit = false)]
        public virtual long? CreateUserId { get; set; }

        [Navigate(NavigateType.OneToOne, nameof(CreateUserId))]
        [AutoGenerateColumn(Text = "创建者",  IsVisibleWhenAdd = false, IsVisibleWhenEdit = false)]
        public virtual UserVO? CreateUser { get; set; }
    }

    public abstract class EntityOrgBase : EntityUserBase
    {
        [AutoGenerateColumn(Text = "组织Id", Ignore = true, IsVisibleWhenAdd = false, IsVisibleWhenEdit = false)]
        public virtual long? OrgId { get; set; }

        [Navigate(NavigateType.OneToOne, nameof(OrgId))]
        [AutoGenerateColumn(Text = "组织",  IsVisibleWhenAdd = false, IsVisibleWhenEdit = false)]
        public virtual Organization? Org { get; set; }
    }



    public abstract class EntityUserTimeBase : IEntityBase
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = false)]
        [AutoGenerateColumn(Ignore = true)]
        public virtual long Id { get; set; }

        [AutoGenerateColumn(Text = "创建时间", IsVisibleWhenAdd = false, IsVisibleWhenEdit = false)]
        public virtual DateTime? CreateTime { get; set; }

        [AutoGenerateColumn(Text = "创建者Id", Ignore = true, IsVisibleWhenAdd = false, IsVisibleWhenEdit = false)]
        public virtual long? CreateUserId { get; set; }

        [Navigate(NavigateType.OneToOne, nameof(CreateUserId))]
        [AutoGenerateColumn(Text = "创建者",  IsVisibleWhenAdd = false, IsVisibleWhenEdit = false)]
        public virtual UserVO? CreateUser { get; set; }


        [AutoGenerateColumn(Text = "更新时间", IsVisibleWhenAdd = false, IsVisibleWhenEdit = false)]
        public virtual DateTime? UpdateTime { get; set; }

        [AutoGenerateColumn(Text = "更新者Id", Ignore = true, IsVisibleWhenAdd = false, IsVisibleWhenEdit = false)]
        public virtual long? UpdateUserId { get; set; }
    }
}
