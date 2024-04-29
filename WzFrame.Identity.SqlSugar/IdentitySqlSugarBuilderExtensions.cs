using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WzFrame.Identity.SqlSugar;

public static class IdentitySqlSugarBuilderExtensions
{
    public static IdentityBuilder AddSqlSugarStores<TContext>(this IdentityBuilder builder)
    where TContext : ISqlSugarClient
    {
        AddStores(builder.Services, builder.UserType, builder.RoleType, typeof(TContext));
        return builder;
    }

    private static void AddStores(IServiceCollection services, Type userType, Type roleType, Type contextType)
    {
        var identityUserType = FindGenericBaseType(userType, typeof(IdentityUser<>));
        if (identityUserType == null)
        {
            throw new InvalidOperationException("AddSqlSugarStores只能由从IdentityUser<TKey>派生的用户调用。");
        }
        var keyType = identityUserType.GenericTypeArguments[0];

        if (roleType != null)
        {
            var identityRoleType = FindGenericBaseType(roleType, typeof(IdentityRole<>));
            if (identityRoleType == null)
            {
                throw new InvalidOperationException("AddSqlSugarStores只能由从IdentityUser<TKey>派生的用户调用。");
            }

            Type userStoreType = null;
            Type roleStoreType = null;
            var identityContext = FindGenericBaseType(contextType, typeof(IdentitySqlSugarScope<,,,,,,,>));
            if (identityContext == null)
            {
                // If its a custom DbContext, we can only add the default POCOs
                userStoreType = typeof(UserStore<,,,>).MakeGenericType(userType, roleType, contextType, keyType);
                roleStoreType = typeof(RoleStore<,,>).MakeGenericType(roleType, contextType, keyType);
            }
            else
            {
                userStoreType = typeof(UserStore<,,,,,,,,>).MakeGenericType(userType, roleType, contextType,
                    identityContext.GenericTypeArguments[2],
                    identityContext.GenericTypeArguments[3],
                    identityContext.GenericTypeArguments[4],
                    identityContext.GenericTypeArguments[5],
                    identityContext.GenericTypeArguments[7],
                    identityContext.GenericTypeArguments[6]);
                roleStoreType = typeof(RoleStore<,,,,>).MakeGenericType(roleType, contextType,
                    identityContext.GenericTypeArguments[2],
                    identityContext.GenericTypeArguments[4],
                    identityContext.GenericTypeArguments[6]);
            }
            services.TryAddScoped(typeof(IUserStore<>).MakeGenericType(userType), userStoreType);
            services.TryAddScoped(typeof(IRoleStore<>).MakeGenericType(roleType), roleStoreType);
        }
        else
        {   // No Roles
            Type userStoreType = null;
            var identityContext = FindGenericBaseType(contextType, typeof(IdentityUserSqlSugarScope<,,,,>));
            if (identityContext == null)
            {
                // If its a custom DbContext, we can only add the default POCOs
                userStoreType = typeof(UserOnlyStore<,,>).MakeGenericType(userType, contextType, keyType);
            }
            else
            {
                userStoreType = typeof(UserOnlyStore<,,,,,>).MakeGenericType(userType, contextType,
                    identityContext.GenericTypeArguments[1],
                    identityContext.GenericTypeArguments[2],
                    identityContext.GenericTypeArguments[3],
                    identityContext.GenericTypeArguments[4]);
            }
            services.TryAddScoped(typeof(IUserStore<>).MakeGenericType(userType), userStoreType);
        }
    }


    private static Type? FindGenericBaseType(Type currentType, Type genericBaseType)
    {
        var type = currentType;
        while (type != null)
        {
            var genericType = type.IsGenericType ? type.GetGenericTypeDefinition() : null;
            if (genericType != null && genericType == genericBaseType)
            {
                return type;
            }
            type = type.BaseType;
        }
        return null;
    }
}