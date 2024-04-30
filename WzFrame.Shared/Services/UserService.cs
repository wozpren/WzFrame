using BootstrapBlazor.Components;
using Masuit.Tools.Core.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WzFrame.Entity.Users;
using WzFrame.Shared.Repository;

namespace WzFrame.Shared.Services
{
    [ServiceInject(ServiceLifetime.Scoped)]
    public class UserService : EntityService<ApplicationUser>
    {
        private readonly IdentityOptions options;

        public UserService(EntityRepository<ApplicationUser> entityRepository, IOptions<IdentityOptions> options) : base(entityRepository)
        {
            this.options = options.Value;
        }

        public async Task<ApplicationUser?> ValidateSecurityStampAsync(ClaimsPrincipal? principal)
        {
            if (principal == null)
            {
                return null;
            }
            var id = principal.FindFirstValue(ClaimTypes.NameIdentifier);
            if(id == null)
            {
                return null;
            }
            long userId = long.Parse(id);
            var user = await entityRepository
                .AsQueryable()
                .Includes(a => a.Roles)
                .SingleAsync(u => u.Id == userId);

            if (user.SecurityStamp == principal.FindFirstValue(options.ClaimsIdentity.SecurityStampClaimType))
            {
                return user;
            }
            return null;
        }

        public async Task<QueryData<ApplicationUser>> OnQueryAsync(QueryPageOptions options)
        {
            RefAsync<int> totalcount = 0;

            var data = await entityRepository.AsQueryable()
            .Includes(a => a.Roles)
            .ToPageListAsync(1, 10, totalcount);
            var result = new QueryData<ApplicationUser>()
            {
                Items = data,
                TotalCount = totalcount,
                IsFiltered = true,
                IsAdvanceSearch = true,
                IsSearch = true,
            };
            return result;

        }

        public async Task<bool> OnSaveAsync(ApplicationUser user, ItemChangedType changedType)
        {
            if (changedType == ItemChangedType.Update)
            {
                user.SecurityStamp = Guid.NewGuid().ToString();
                return await entityRepository
                .Context
                .Updateable(user)
                .ExecuteCommandWithOptLockAsync() > 0;
            }
            else
            {

            }
            return false;
        }

        public async Task<bool> OnDeleteAsync(IEnumerable<ApplicationUser> items)
        {
            return await entityRepository.Context
            .DeleteNav<ApplicationUser>(items.ToList())
            .Include(x => x.Roles, new DeleteNavOptions()
            {
                ManyToManyIsDeleteA = true
            })
            .ExecuteCommandAsync();
        }

        public Task SetRole(ApplicationUser applicationUser)
        {
            return Task.CompletedTask;
        }

    }
}
