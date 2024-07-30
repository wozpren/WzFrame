using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using WzFrame.Services;

namespace WzFrame.Components
{
    public abstract class WComponentBase : ComponentBase
    {
        [Inject]
        protected AuthenticationStateProvider? StateProvider { get; set; }

        [Inject]
#pragma warning disable CS8618
        protected NavigationManager NavigationManager { get; set; }
#pragma warning restore CS8618


    }
}
