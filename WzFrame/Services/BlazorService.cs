using BootstrapBlazor.Components;
using Masuit.Tools.Core.AspNetCore;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using WzFrame.Entity.System;
using WzFrame.Shared.Services;

namespace WzFrame.Services
{
    [ServiceInject(ServiceLifetime.Scoped)]
    public class BlazorService
    {
        private List<MenuOption>? MenuOptionsTree { get; set; }
        private List<MenuOption>? MenuOptions { get; set; }

        private readonly MenuService menuService;

        public BlazorService(MenuService menuService)
        {
            this.menuService = menuService;
        }

        private void SetMenuOptions(List<MenuOption> menuOptions)
        {
            MenuOptionsTree = menuOptions;
            MenuOptions = new List<MenuOption>(menuOptions);
            for (int i = 0; i < MenuOptions.Count; i++)
            {
                if (MenuOptions[i].Children != null)
                {
                    MenuOptions.AddRange(MenuOptions[i].Children);
                }
            }
        }

        public async ValueTask<List<MenuOption>> GetMenuOptionsTree(AuthenticationState? state)
        {
            if(MenuOptionsTree == null)
            {                
                MenuOptionsTree = await menuService.GetTree(state);
                SetMenuOptions(MenuOptionsTree);
            }
            return MenuOptionsTree;
        }

        public async ValueTask<List<MenuOption>> GetMenuOptions(AuthenticationState? state)
        {
            if(state == null)
            {
                return MenuOptions ?? new List<MenuOption>();
            }
            if (MenuOptions == null)
            {
                MenuOptionsTree = await menuService.GetTree(state);
                SetMenuOptions(MenuOptionsTree);
            }
            return MenuOptions ?? new List<MenuOption>();
        }

        public async ValueTask<List<MenuItem>> GetMenuItems(AuthenticationState? state)
        {
            var menus = await GetMenuOptionsTree(state);
            return BuildMenu(menus);
        }

        protected List<MenuItem> BuildMenu(List<MenuOption>? menus, MenuItem? parent = null)
        {

            if (menus == null)
            {
                return new List<MenuItem>();
            }

            menus.Sort((a, b) => a.Order - b.Order);

            var result = new List<MenuItem>();
            foreach (var menu in menus)
            {
                if (menu.Type == MenuType.Button)
                {
                    continue;
                }
                var item = new MenuItem
                {
                    Id = menu.Id.ToString(),
                    Text = menu.Name,
                    Icon = menu.Icon,
                    Url = menu.Path,
                };
                if (menu.Children != null)
                {
                    item.Items = BuildMenu(menu.Children.ToList(), item);
                }

                if (parent != null)
                {
                    item.Parent = parent;
                    item.ParentId = parent.Id;
                }


                result.Add(item);
            }

            return result;
        }
    }
}
