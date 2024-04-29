using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WzFrame.Shared.Core
{
    public static class InternalApp
    {
        internal static IServiceCollection InternalServices { get; private set; }

        internal static IServiceProvider RootServices { get; private set; }

        internal static IWebHostEnvironment WebHostEnvironment { get; private set; }

        internal static IHostEnvironment HostEnvironment { get; private set; }

        internal static IConfiguration Configuration { get; set; }


        public static void ConfigureApp(this WebApplicationBuilder builder)
        {
            InternalServices = builder.Services;
            HostEnvironment = builder.Environment;
            WebHostEnvironment = builder.Environment;
        }

        public static void ConfigureApp(this IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public static void ConfigureApp(this IHost app)
        {
            RootServices = app.Services;
        }



    }
}
