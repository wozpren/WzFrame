using Microsoft.AspNetCore.Components;

namespace WzFrame.Extension
{
    public static class BlazorExtension
    {

        public static string GetDomain(this NavigationManager navigationManager)
        {
            var uri = navigationManager.ToAbsoluteUri(navigationManager.Uri);
            return uri.Host;
        }

    }
}
