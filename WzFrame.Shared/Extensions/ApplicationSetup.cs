using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WzFrame.Shared.Extensions
{
    public static class ApplicationSetup
    {
        public static void UseApplicationSetup(this WebApplication app)
        {
            app.Lifetime.ApplicationStarted.Register(() =>
            {
                Console.WriteLine("Application started");
                App.IsRun = true;
            });
            app.Lifetime.ApplicationStopping.Register(() =>
            {
                Console.WriteLine("Application stopping");
                App.IsRun = false;
            });
        }

    }
}
