using BootstrapBlazor.Components;
using Masuit.Tools;
using Masuit.Tools.Core.AspNetCore;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using SixLabors.ImageSharp.Drawing.Processing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WzFrame.Entity.System;
using WzFrame.Shared.Extensions;
using WzFrame.Shared.Repository;

namespace WzFrame.Shared.Services
{
    [ServiceInject(ServiceLifetime.Scoped)]
    public class MenuService : EntityService<MenuOption>
    {
        private readonly IMemoryCache cache;

        public MenuService(EntityRepository<MenuOption> entityRepository, WebService webService, IMemoryCache cache) : base(entityRepository, webService)
        {
            this.cache = cache;
        }

        public Task<List<MenuOption>> GetTree()
        {
            return entityRepository.AsQueryable()
                .ToTreeAsync(it => it.Children, it => it.ParentId, 0);
        }

        public async Task<List<MenuOption>> GetTree(AuthenticationState? state)
        {
            var meun = await GetTree();
            if (state != null)
            {
                var roles = state.User.Claims
                    .Where(x => x.Type == ClaimTypes.Role)
                    .Select(i=> i.Value.ToLower())
                    .ToList();
                FilterMenu(meun, roles);
            }
            return meun;
        }

        private void FilterMenu(List<MenuOption> tree, IEnumerable<string> roles)
        {
            for (var i = 0; i < tree.Count; i++)
            {
                if (tree[i].Permission != null && tree[i].Permission!.Count > 0)
                {
                    if (!tree[i].Permission!.Contains(roles))
                    {
                        tree.RemoveAt(i);
                        i--;
                        continue;
                    }
                }
                if (tree[i].Children != null)
                {
                    FilterMenu(tree[i].Children, roles);
                }
            }
        }
    }
}
