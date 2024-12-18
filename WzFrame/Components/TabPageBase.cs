using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using WzFrame.Entity.DTO;
using WzFrame.Entity.System;
using WzFrame.Services;
using WzFrame.Shared.Extensions;
namespace WzFrame.Components
{

    public class TabPageBase : WComponentBase
    {
        [CascadingParameter]
        [NotNull]
        public Tab Tab { get; set; }

        [CascadingParameter]
        [NotNull]
        public TabItem TabItem { get; set; }

        [Inject]
        [NotNull]
        protected BlazorService BlazorService { get; set; }

        protected List<MenuOption>? MenuOptions { get; set; }

        [CascadingParameter]
        protected UserVO? User { get; set; }

        protected MenuOption? MenuOption { get; set; }

        protected AuthenticationState? CurrentUser { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            if (StateProvider != null)
            {
                CurrentUser = await StateProvider.GetAuthenticationStateAsync();

                if (CurrentUser == null) return;

                if (MenuOptions == null)
                    MenuOptions = await BlazorService.GetMenuOptions(CurrentUser);

                MenuOption = MenuOptions.FirstOrDefault(x => x.Path == "/" + TabItem.Url);

                if (MenuOption != null)
                {
                    TabItem.SetHeader(MenuOption.Name, MenuOption.Icon);
                    if (!CheckPermission(CurrentUser.User, MenuOption.Permission))
                    {
                        NavigationManager.NavigateTo("/");
                    }
                }
                else
                {
                    NavigationManager.NavigateTo("/");
                }
            }
            else 
            {
                NavigationManager.NavigateTo("/");
            }
        }

        protected bool CheckPermission(ClaimsPrincipal user, List<string>? permission)
        {
            if (permission == null || permission.Count == 0)
            {
                return true;
            }
            foreach (var item in permission)
            {
                foreach (var claim in user.Identities)
                {
                    if (claim.HasClaim(c => c.Value.ToLower() == item.ToLower()))
                    {
                        return true;
                    }
                }
            }
            return false;

        }

    }
}
