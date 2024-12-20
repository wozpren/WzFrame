﻿using AngleSharp.Dom;
using BootstrapBlazor.Components;
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
    public class EntityService<TEntity> : IDataService<TEntity> where TEntity : class, IEntityBase, new()
    {

        public readonly EntityRepository<TEntity> entityRepository;

        protected readonly WebService webService;

        public EntityService(EntityRepository<TEntity> entityRepository, WebService webService)
        {
            this.entityRepository = entityRepository;
            this.webService = webService;
        }

        public async Task<QueryData<TEntity>> QueryAsync(QueryPageOptions queryPageOptions)
        {
            RefAsync<int> totalcount = 0;

            var data = await entityRepository.AsQueryable()
                .OrderByDescending(te => te.Id)
                .ToPageListAsync(queryPageOptions.PageIndex, queryPageOptions.PageItems, totalcount);

            var result = new QueryData<TEntity>()
            {
                Items = data,
                TotalCount = totalcount,
                IsFiltered = false,
                IsAdvanceSearch = false,
                IsSearch = false,
            };
            return result;
        }


        public async Task<List<TEntity>> GetAll()
        {
            return await entityRepository.GetListAsync();
        }


        public async Task<TEntity> GetAsync(long id)
        {
            return await entityRepository.GetByIdAsync(id);
        }


        public async virtual Task<long> AddAsync(TEntity entity)
        {
            if (entity == null) return 0;

            if(entity is EntityUserBase userEntity)
            {
                userEntity.CreateUserId = webService.CurrentUser?.Id;
            }
            else if(entity is EntityUserTimeBase entityBase)
            {
                entityBase.CreateUserId = webService.CurrentUser?.Id;
                entityBase.CreateTime = DateTime.Now;
            }

            if (entity.Id == 0)
            {
                return await entityRepository.InsertReturnSnowflakeIdAsync(entity);
            }
            else
            {
                await entityRepository.InsertAsync(entity);
                return entity.Id;
            }
        }

        public async virtual Task<bool> UpdateAsync(TEntity entity)
        {
            if (entity is EntityUserTimeBase entityBase)
            {
                entityBase.UpdateTime = DateTime.Now;
                entityBase.UpdateUserId = webService.CurrentUser?.Id;
            }

            return await entityRepository.UpdateAsync(entity);
        }

        public async virtual Task<bool> DeletesAsync(IEnumerable<TEntity> entitys)
        {
            return await entityRepository.DeleteAsync(entitys.ToList());
        }


        Task<bool> IDataService<TEntity>.AddAsync(TEntity model)
        {
            return Task.FromResult(true);
        }

        public async Task<bool> SaveAsync(TEntity model, ItemChangedType changedType)
        {
            if (changedType == ItemChangedType.Add)
            {
                return await AddAsync(model) > 0;
            }
            else if (changedType == ItemChangedType.Update)
            {
                return await UpdateAsync(model);
            }

            return true;
        }

        public Task<bool> DeleteAsync(IEnumerable<TEntity> models)
        {
            return DeletesAsync(models);
        }
    }
}
