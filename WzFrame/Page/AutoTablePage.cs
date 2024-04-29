
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System.Reflection;
using WzFrame.Components;
using WzFrame.Entity.System;

namespace WzFrame.Page
{
    [Route("/auto/{entity}")]
    public class AutoTablePage : TabPageBase
    {

        [Parameter]
        public string? Entity { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }


        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);
            if (MenuOption != null)
            {
                var AssemblyName = MenuOption.Assembly;
                var fullName = MenuOption.ClassName;
                if (Entity != null && AssemblyName != null && fullName != null)
                {
                    Type genericType = typeof(TablePageBase<>);
                    var typeAssembly = Assembly.Load(AssemblyName);
                    var type = typeAssembly.GetType(fullName);
                    if (type != null)
                    {
                        Type constructedType = genericType.MakeGenericType(type);
                        builder.OpenComponent(1, constructedType);
                        builder.CloseComponent();
                        return;
                    }
                }
            }

            Build404(builder);
        }

        protected virtual void Build404(RenderTreeBuilder builder)
        {
            builder.OpenElement(0, "h1");
            builder.AddContent(1, "404 Not Found");
            builder.CloseElement();
        }

    }
}
