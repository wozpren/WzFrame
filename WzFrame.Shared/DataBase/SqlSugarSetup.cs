using Mapster;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WzFrame.Entity.Attributes;
using WzFrame.Entity.Configuration;
using WzFrame.Entity.Consts;
using WzFrame.Entity.System;
using WzFrame.Entity.Users;
using WzFrame.Shared.Extensions;
using Yitter.IdGenerator;

namespace WzFrame.Shared.DataBase;
public static class SqlSugarSetup
{
    public static void AddSqlSugar(this WebApplicationBuilder builder)
    {
        var options = new IdGeneratorOptions(1);
        YitIdHelper.SetIdGenerator(options);

        SnowFlakeSingle.WorkId = options.WorkerId;
        StaticConfig.CustomSnowFlakeFunc = YitIdHelper.NextId;


        DatabaseConfig db = AppSettings.Get<DatabaseConfig>()!;

        ApplicationDbContext sqlSugar = new ApplicationDbContext(db.ConnectionConfigs.Adapt<List<ConnectionConfig>>(),
        sql =>
        {
            foreach (var dbConfig in db.ConnectionConfigs)
            {
                var dbProvider = sql.GetConnectionScope(dbConfig.ConfigId);
                SetDbAop(dbProvider);
            }
        });


        builder.Services.AddHttpContextAccessor();
        //builder.Services.AddSingleton<ISqlSugarClient>(sqlSugar);
        builder.Services.AddSingleton(sqlSugar);

        foreach (var dbConfig in db.ConnectionConfigs)
        {
            if (dbConfig.Enable)
            {
                InitDatabase(sqlSugar, dbConfig);
            }
        }

        InitData(sqlSugar, db);

    }

    public static void SetDbAop(SqlSugarScopeProvider db)
    {
        var config = db.CurrentConnectionConfig;
        db.Ado.CommandTimeOut = 30;

        if (true)
        {
            db.Aop.OnLogExecuting = (sql, pars) =>
            {
                var fileName = db.Ado.SqlStackTrace.FirstFileName; // 文件名
                var fileLine = db.Ado.SqlStackTrace.FirstLine; // 行号
                var firstMethodName = db.Ado.SqlStackTrace.FirstMethodName; // 方法名

                var log = $"【{DateTime.Now}——执行SQL】\r\n{UtilMethods.GetSqlString(config.DbType, sql, pars)}\r\n【所在文件名】：{fileName}\r\n【代码行数】：{fileLine}\r\n【方法名】：{firstMethodName}\r\n";
                var originColor = Console.ForegroundColor;
                if (sql.StartsWith("SELECT", StringComparison.OrdinalIgnoreCase))
                    Console.ForegroundColor = ConsoleColor.Green;
                if (sql.StartsWith("UPDATE", StringComparison.OrdinalIgnoreCase) || sql.StartsWith("INSERT", StringComparison.OrdinalIgnoreCase))
                    Console.ForegroundColor = ConsoleColor.Yellow;
                if (sql.StartsWith("DELETE", StringComparison.OrdinalIgnoreCase))
                    Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(log);
                Console.ForegroundColor = originColor;
                //App.PrintToMiniProfiler("SqlSugar", "Info", log);
            };
            db.Aop.OnError = ex =>
            {
                if (ex.Parametres == null) return;
                var log = $"【{DateTime.Now}——错误SQL】\r\n{UtilMethods.GetSqlString(config.DbType, ex.Sql, (SugarParameter[])ex.Parametres)}\r\n";
                var originColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine(log);
                Console.ForegroundColor = originColor;
                //App.PrintToMiniProfiler("SqlSugar", "Error", log);
            };
            db.Aop.OnLogExecuted = (sql, pars) =>
            {
                // 执行时间超过5秒
                if (db.Ado.SqlExecutionTime.TotalSeconds > 5)
                {
                    var fileName = db.Ado.SqlStackTrace.FirstFileName; // 文件名
                    var fileLine = db.Ado.SqlStackTrace.FirstLine; // 行号
                    var firstMethodName = db.Ado.SqlStackTrace.FirstMethodName; // 方法名
                    var log = $"【{DateTime.Now}——超时SQL】\r\n【所在文件名】：{fileName}\r\n【代码行数】：{fileLine}\r\n【方法名】：{firstMethodName}\r\n" + $"【SQL语句】：{UtilMethods.GetSqlString(config.DbType, sql, pars)}";
                    var originColor = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine(log);
                    Console.ForegroundColor = originColor;
                    //App.PrintToMiniProfiler("SqlSugar", "Slow", log);
                }
            };
        }
    }

    public static void InitDatabase(SqlSugarScope db, WzConnectionConfig dbConfig)
    {
        SqlSugarScopeProvider dbProvider = db.GetConnectionScope(dbConfig.ConfigId);

        if (dbConfig.InitDb)
        {
            if (dbConfig.DbType != SqlSugar.DbType.Oracle)
                dbProvider.DbMaintenance.CreateDatabase();
        }

        if (dbConfig.InitTable)
        {
            var entityTypes = App.EffectiveTypes.Where(u => !u.IsInterface && !u.IsAbstract && u.IsClass && u.IsDefined(typeof(SugarTable), false) && !u.IsDefined(typeof(DoNotCreateTableAttribute)));

            //if (dbConfig.ConfigId.ToString() == DatabaseConst.SystemDbConfigId)
            //{
            //    entityTypes = entityTypes.Where(u => u.IsDefined(typeof(SysTableAttribute)));
            //}
            //else
            //{
            //    entityTypes = entityTypes.Where(u => !u.IsDefined(typeof(SysTableAttribute)));
            //}


            var elist = entityTypes.ToList();
            foreach (var entityType in elist)
            {
                if (entityType.GetCustomAttribute<SplitTableAttribute>() == null)
                    dbProvider.CodeFirst.InitTables(entityType);
                else
                    dbProvider.CodeFirst.SplitTables().InitTables(entityType);
            }
        }

    }

    public static void InitData(SqlSugarScope db, DatabaseConfig opt)
    {
        if (opt.InitMenu)
        {
            InitMenuFormPage(db);

            var roleList = new List<Role>
            {
                new Role
                {
                    Id = 139132739219525,
                    Name = "admin",
                    DisplayName = "超级管理员",
                    NormalizedName = "超级管理员",
                    ConcurrencyStamp = Guid.NewGuid().ToString(),                   
                }
            };

            var userList = new List<ApplicationUser>
            {
                new ApplicationUser
                {
                    Id = 539132739219525,
                    UserName = "wozpren",
                    NormalizedUserName = "WOZPREN",
                    Description = "超级管理员",
                    EmailConfirmed = true,
                    PasswordHash = "AQAAAAIAAYagAAAAEFJMSDfHCioZPFHTyjvXgGZs/6Mg+DpW+jP0OVJNylEIbTrYKbCyB/iOB4U37J1f6Q==",
                    SecurityStamp = "VKLACJJM6SJACUYIVPLZXANHSXOUT5EK",
                }
            };

            var userRoles = new List<IdentityUserRole>
            {
                new IdentityUserRole
                {
                    RoleId = 139132739219525,
                    UserId = 539132739219525,
                }
            };


            db.Storageable(roleList).ExecuteCommandAsync();
            db.Storageable(userList).ExecuteCommandAsync();
            db.Storageable(userRoles).ExecuteCommandAsync();
        }






        //var storage = db.Storageable(list).ToStorage();
        //storage.AsInsertable.ExecuteCommand();
        //storage.AsUpdateable.ExecuteCommand();

    }


    public static void InitMenuFormPage(SqlSugarScope db)
    {
         var pages = App.EffectiveTypes
            .Where(u => !u.IsInterface && !u.IsAbstract && u.IsClass && u.IsDefined(typeof(MenuPageOptionAttribute))).ToList();
        var menus = new List<MenuOption>();
        foreach (Type page in pages)
        {
            if (page.GetCustomAttribute<RouteAttribute>() is var route)
            {
                var path = route!.Template;
                var opts = page.GetCustomAttributes<MenuPageOptionAttribute>().ToList();
                foreach (var opt in opts)
                {
                    var menuOpt = new MenuOption()
                    {
                        Id = opt.Id,
                        Type = opt.Type,
                        Name = opt.Name,
                        Icon = opt.Icon,
                        Path = opt.Path,
                        Order = opt.Order,
                        ParentId = opt.ParentId,
                    };
                    if(opt.Permission != null)
                    {
                        menuOpt.Permission = opt.Permission.Split(',').ToList();
                    }
                    else
                    {
                        menuOpt.Permission = new List<string>();
                    }
                    if(menuOpt.Path == null && opt.Type != MenuType.Directory)
                    {
                        menuOpt.Path = path;
                    }
                    menus.Add(menuOpt);
                }
            }
            else
            {
                continue;
            }
        }

        db.Storageable(menus).ExecuteCommandAsync();
    }
}