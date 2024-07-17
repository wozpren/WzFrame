using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BootstrapBlazor.Components;
using Masuit.Tools.Core.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SqlSugar;
using WzFrame.Entity.Configuration;
using WzFrame.Entity.Users;
using WzFrame.Shared.Repository;

namespace WzFrame.Shared.Services
{
    [ServiceInject(ServiceLifetime.Scoped)]
    public class UserService : EntityService<ApplicationUser>
    {
        private readonly IdentityOptions options;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly string DefaultPassword;

        public UserService(
            EntityRepository<ApplicationUser> entityRepository,
            UserManager<ApplicationUser> UserManager,
            IOptions<IdentityOptions> options,
            WebService webService
        )
            : base(entityRepository, webService)
        {
            this.options = options.Value;
            userManager = UserManager;
            DefaultPassword = AppSettings.Get<WebsiteConfig>().DefaultPassword;
        }

        public async Task<ApplicationUser?> ValidateSecurityStampAsync(ClaimsPrincipal? principal)
        {
            if (principal == null)
            {
                return null;
            }
            var id = principal.FindFirstValue(ClaimTypes.NameIdentifier);
            if (id == null)
            {
                return null;
            }
            long userId = long.Parse(id);
            var user = await entityRepository
                .AsQueryable()
                .Includes(a => a.Roles)
                .SingleAsync(u => u.Id == userId);

            if (
                user.SecurityStamp
                == principal.FindFirstValue(options.ClaimsIdentity.SecurityStampClaimType)
            )
            {
                return user;
            }
            return null;
        }

        public async Task<QueryData<ApplicationUser>> OnQueryAsync(QueryPageOptions options)
        {
            RefAsync<int> totalcount = 0;

            var data = await entityRepository
                .AsQueryable()
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
                        .Context.Updateable(user)
                        .ExecuteCommandWithOptLockAsync() > 0;
            }
            else
            {
                var result = await userManager.CreateAsync(user, DefaultPassword);
                return result.Succeeded;
            }
        }

        public async Task<bool> OnDeleteAsync(IEnumerable<ApplicationUser> items)
        {
            return await entityRepository
                .Context.DeleteNav<ApplicationUser>(items.ToList())
                .Include(x => x.Roles, new DeleteNavOptions() { ManyToManyIsDeleteA = true })
                .ExecuteCommandAsync();
        }

        public async Task<IdentityResult> ChangePassword(string oldpassword ,string newpassword)
        {
            var userdto = webService.CurrentUser;
            if (userdto == null) throw new Exception("用户未登录");

            ApplicationUser? user = await userManager.FindByIdAsync(userdto.Id.ToString());
            if (user == null) throw new Exception("用户不存在");


            return await userManager.ChangePasswordAsync(user, oldpassword, newpassword);
        }

        public Task SetRole(ApplicationUser applicationUser)
        {
            return Task.CompletedTask;
        }
    }
}
