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
using WzFrame.Entity.Users.DTO;

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


    [SysTable]
    [Tenant(DatabaseConst.SystemDbConfigId)]
    public abstract class SysEntity : IEntityBase
    {

        [SugarColumn(IsPrimaryKey = true, IsIdentity = false)]
        public virtual long Id { get; set; }
    }

    public abstract class EntityUserBase : IEntityBase
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = false)]
        public virtual long Id { get; set; }


        [Navigate(NavigateType.OneToOne, nameof(Id))]
        public virtual UserMessage? CreateUser { get; set; }
    }



    public abstract class EntityUserTimeBase : IEntityBase
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = false)]
        public virtual long Id { get; set; }

        public virtual DateTime? CreateTime { get; set; }
        public virtual long? CreateUserId { get; set; }

        [Navigate(NavigateType.OneToOne, nameof(CreateUserId))]
        public virtual UserMessage? CreateUser { get; set; }


        public virtual DateTime? UpdateTime { get; set; }
        public virtual long? UpdateUserId { get; set; }
    }
}
