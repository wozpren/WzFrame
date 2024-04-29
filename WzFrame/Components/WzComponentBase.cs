using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using WzFrame.Services;

namespace WzFrame.Components
{
    public abstract class WzComponentBase : ComponentBase
    {
        [Inject]
        protected AuthenticationStateProvider? StateProvider { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }


    }
}
