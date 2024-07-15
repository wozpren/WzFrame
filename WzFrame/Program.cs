using Masuit.Tools.Config;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Threading.RateLimiting;
using WzFrame.Entity.Users;
using WzFrame.Identity.SqlSugar;
using WzFrame.Services;
using WzFrame.Shared;
using WzFrame.Shared.Core;
using WzFrame.Shared.DataBase;
using WzFrame.Shared.Extensions;
using WzFrame.Shared.Identity;
using WzFrame.Shared.Repository;
using WzFrame.Shared.Services;
using Masuit.Tools;
using Masuit.Tools.Core.AspNetCore;
using Quartz;
using Microsoft.AspNetCore.ResponseCompression;
using WzFrame.Shared.Hubs;
using Microsoft.AspNetCore.SignalR;
using OnceMi.AspNetCore.OSS;

var builder = WebApplication.CreateBuilder(args);

builder.AddConfigs();
builder.Configuration.AddToMasuitTools();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();


builder.Services.Configure<IdentityOptions>(config =>
{
    config.SignIn.RequireConfirmedEmail = false;
    config.SignIn.RequireConfirmedPhoneNumber = false;
    config.Password = new PasswordOptions
    {
        RequireNonAlphanumeric = false,
    };
});

builder.ConfigureApp();
builder.AddSqlSugar();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = IdentityConstants.ApplicationScheme;
    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
})
    .AddIdentityCookies();

builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<Role>()
    .AddRoleManager<RoleManager<Role>>()
    .AddSqlSugarStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<IUserIdProvider, UserIdProvider>();
builder.Services.AutoRegisterServices();

builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
          new[] { "application/octet-stream" });
});

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();
builder.Services.AddScoped(typeof(EntityRepository<>));
builder.Services.AddScoped(typeof(EntityService<>));


builder.Services.AddMemoryCache();
builder.Services.AddCaptcha(builder.Configuration);

builder.Services.AddQuartz();
builder.Services.AddQuartzHostedService(options =>
{    
    options.WaitForJobsToComplete = true;
});

builder.Services.AddBootstrapBlazor(null, opt =>
{
    opt.IgnoreLocalizerMissing = true;
});
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "WzFrame", Version = "v1" });
});

builder.Services.AddHttpClient("eshop", p =>
{
    p.BaseAddress = new Uri("http://wwww.553vip.com");
});

builder.Services.AddOSSService("OSSProvider");



var app = builder.Build();
app.ConfigureApp();
app.UseApplicationSetup();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseResponseCompression();

    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    app.UseSwagger();

    // 启用中间件服务对swagger-ui，指定Swagger JSON终结点
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    });
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<WzFrame.Page.App>()
    .AddInteractiveServerRenderMode();

app.MapHub<BlazorHub>("/client");

app.MapAdditionalIdentityEndpoints();
app.MapControllers();
app.Run();