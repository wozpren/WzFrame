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
        public List<MenuOption>? MenuOptionsTree { get; set; }
        public List<MenuOption>? MenuOptions { get; set; }

        private readonly MenuService menuService;

        public BlazorService(MenuService menuService)
        {
            this.menuService = menuService;
        }

        public void SetMenuOptions(List<MenuOption> menuOptions)
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


        public async ValueTask<MenuOption?> GetMenuOption(string path)
        {
            if(MenuOptions == null)
            {
               await GetMenuOptionsTree();
            }
            return MenuOptions?.FirstOrDefault(x => x.Path == "/" + path);
        }

        public async ValueTask<List<MenuOption>> GetMenuOptionsTree()
        {
            if (MenuOptionsTree == null)
            {
                MenuOptionsTree = await menuService.GetTree();
                SetMenuOptions(MenuOptionsTree);
            }
            return MenuOptionsTree;
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

    }
}
