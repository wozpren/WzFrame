using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using WzFrame.Entity.DTO;
using WzFrame.Entity.System;
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

        [CascadingParameter]
        protected List<MenuOption>? MenuOptions { get; set; }

        [CascadingParameter]
        protected UserDTO? User { get; set; }

        protected MenuOption? MenuOption { get; set; }

        protected AuthenticationState? CurrentUser { get; set; }

        protected override void OnParametersSet()
        {
            if (MenuOptions != null)
            {
                MenuOption = MenuOptions.FirstOrDefault(x => x.Path == "/" + TabItem.Url);
                if (MenuOption != null)
                    TabItem.SetHeader(MenuOption.Name, MenuOption.Icon);
            }
        }
        protected override async Task OnParametersSetAsync()
        {
            if (StateProvider != null && MenuOption != null)
            {
                CurrentUser = await StateProvider.GetAuthenticationStateAsync();
                if (!CheckPermission(CurrentUser.User, MenuOption.Permission))
                {
                    NavigationManager.NavigateTo("/");
                }
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
