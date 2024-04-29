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

        public MenuService(EntityRepository<MenuOption> entityRepository, IMemoryCache cache) : base(entityRepository)
        {
            this.cache = cache;
        }


        public async ValueTask<List<MenuOption>> GetTree()
        {
            if(cache.TryGetValue("menu", out List<MenuOption>? menuOptionsTree))
            {
                return menuOptionsTree!;
            }

            var tree = await entityRepository.AsQueryable().ToTreeAsync(it => it.Children, it=> it.ParentId, 0);
            var list = new List<MenuOption>();
            GetMenu(list, tree);

            cache.Set("menu", tree, TimeSpan.FromDays(1));
            return tree;
        }

        private IEnumerable<MenuOption> GetMenu(List<MenuOption> list, IEnumerable<MenuOption> tree)
        {
            list.AddRange(tree);
            foreach (var item in tree)
            {
                if (item.Children != null)
                {
                    GetMenu(list, item.Children);
                }
            }
            return tree;
        }


        public async Task<List<MenuOption>> GetTree(AuthenticationState? state)
        {
            List<MenuOption>? tree = await GetTree().DeepClone();
            var newMenu = new List<MenuOption>();
            if (state != null)
            {
                var roles = state.User.Claims
                    .Where(x => x.Type == ClaimTypes.Role)
                    .Select(i=> i.Value.ToLower())
                    .ToList();
                FilterMenu(tree, roles);
            }
            return tree;
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
