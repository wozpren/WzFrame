using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SqlSugar;
using System.Reflection;
using WzFrame.Id.Components;
using WzFrame.Id.Components.Account;
using WzFrame.Id.Data;
using WzFrame.Identity.SqlSugar;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

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

ApplicationDbContext Db = new ApplicationDbContext(new ConnectionConfig()
{
    ConfigId = "Default",
    ConnectionString = "server=localhost;user=root;database=test_id;password=Admin@123",
    DbType = DbType.MySql,
    IsAutoCloseConnection = true,
    ConfigureExternalServices = new ConfigureExternalServices()
    {
        EntityService = (c, p) =>
        {
            if (p.IsPrimarykey == false && new NullabilityInfoContext()
             .Create(c).WriteState is NullabilityState.Nullable)
            {
                p.IsNullable = true;
            }
        }
    }
}, db =>
{
    db.Aop.OnLogExecuting = (sql, pars) =>
    {

        //获取原生SQL推荐 5.1.4.63  性能OK
        Console.WriteLine(UtilMethods.GetNativeSql(sql, pars));

        //获取无参数化SQL 对性能有影响，特别大的SQL参数多的，调试使用
        //Console.WriteLine(UtilMethods.GetSqlString(DbType.SqlServer,sql,pars))

    };
});



builder.Services.AddSingleton(Db);
Db.DbMaintenance.CreateDatabase();

Db.CodeFirst.InitTables<ApplicationUser>();
Db.CodeFirst.InitTables<IdentityRole>();
Db.CodeFirst.InitTables<IdentityUserClaim>();
Db.CodeFirst.InitTables<IdentityUserRole>();
Db.CodeFirst.InitTables<IdentityUserLogin>();
Db.CodeFirst.InitTables<IdentityRoleClaim>();
Db.CodeFirst.InitTables<IdentityUserToken>();



//var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseSqlServer(connectionString));
//builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddSqlSugarStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.UseMigrationsEndPoint();
}
else
{
}
app.UseExceptionHandler("/Error", createScopeForErrors: true);


app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

app.Run();
