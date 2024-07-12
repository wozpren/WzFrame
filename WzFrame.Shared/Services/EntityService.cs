using BootstrapBlazor.Components;
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
using WzFrame.Entity.Interfaces;
using WzFrame.Entity.Users;
using WzFrame.Shared.Repository;

namespace WzFrame.Shared.Services
{
    public class EntityService<TEntity> where TEntity : class, IEntityBase, new()
    {

        protected readonly EntityRepository<TEntity> entityRepository;

        public EntityService(EntityRepository<TEntity> entityRepository)
        {
            this.entityRepository = entityRepository;
        }


        public async Task<QueryData<TEntity>> QueryAsync(QueryPageOptions queryPageOptions)
        {
            RefAsync<int> totalcount = 0;

            var data = await entityRepository.AsQueryable().ToPageListAsync(queryPageOptions.PageIndex, queryPageOptions.PageItems, totalcount);
            var result = new QueryData<TEntity>()
            {
                Items = data,
                TotalCount = totalcount,
                IsFiltered = true,
                IsAdvanceSearch = true,
                IsSearch = true,
            };
            return result;
        }




        public async Task<TEntity> GetAsync(long id)
        {
            return await entityRepository.GetByIdAsync(id);
        }


        public async virtual Task<long> AddAsync(TEntity menu)
        {
            return await entityRepository.InsertReturnSnowflakeIdAsync(menu);
        }

        public async virtual Task<bool> UpdateAsync(TEntity menu)
        {
            return await entityRepository.UpdateAsync(menu);
        }

        public async virtual Task<bool> DeletesAsync(IEnumerable<TEntity> entitys)
        {
            return await entityRepository.DeleteAsync(entitys.ToList());
        }

    }
}
