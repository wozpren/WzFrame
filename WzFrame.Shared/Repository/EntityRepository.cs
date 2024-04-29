using Masuit.Tools.Core.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WzFrame.Entity;
using WzFrame.Entity.Attributes;
using WzFrame.Shared.DataBase;

namespace WzFrame.Shared.Repository
{
    public class EntityRepository<TEntity> : SimpleClient<TEntity> where TEntity : class, IEntityBase, new()
    {

        public EntityRepository(ApplicationDbContext db)
        {
            base.Context = db;
        }

    }
}
