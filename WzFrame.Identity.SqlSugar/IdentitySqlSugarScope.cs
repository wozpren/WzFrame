using Microsoft.AspNetCore.Identity;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WzFrame.Identity.SqlSugar;

public class IdentitySqlSugarScope : IdentitySqlSugarScope<IdentityUser>
{
    public IdentitySqlSugarScope(ConnectionConfig config) : base(config)
    {
    }
}

public class IdentitySqlSugarScope<TUser> : IdentitySqlSugarScope<TUser, IdentityRole, string>
    where TUser : IdentityUser, new()
{
    public IdentitySqlSugarScope(ConnectionConfig config) : base(config)
    {
    }

    public IdentitySqlSugarScope(ConnectionConfig config, Action<SqlSugarClient> action) : base(config, action)
    {
    }
}

public class IdentitySqlSugarScope<TUser, TRole, TKey> : IdentitySqlSugarScope<TUser, TRole, TKey, IdentityUserClaim<TKey>, IdentityUserRole<TKey>, IdentityUserLogin<TKey>, IdentityRoleClaim<TKey>, IdentityUserToken<TKey>>
    where TUser : IdentityUser<TKey> , new()
    where TRole : IdentityRole<TKey>, new()
    where TKey : IEquatable<TKey>
{
    public IdentitySqlSugarScope(ConnectionConfig config) : base(config)
    {
    }

    public IdentitySqlSugarScope(ConnectionConfig config, Action<SqlSugarClient> action) : base(config, action)
    {
    }
}

public abstract class IdentitySqlSugarScope<TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken> : IdentityUserSqlSugarScope<TUser, TKey, TUserClaim, TUserLogin, TUserToken>
    where TUser : IdentityUser<TKey>, new()
    where TRole : IdentityRole<TKey>, new()
    where TKey : IEquatable<TKey>
    where TUserClaim : IdentityUserClaim<TKey>, new()
    where TUserRole : IdentityUserRole<TKey>, new()
    where TUserLogin : IdentityUserLogin<TKey>, new()
    where TRoleClaim : IdentityRoleClaim<TKey>, new()
    where TUserToken : IdentityUserToken<TKey>, new()
{

    public IdentitySqlSugarScope(ConnectionConfig config) : base(config)
    {
        Roles = new SimpleClient<TRole>();
        Roles.Context = this;

        UserRoles = new SimpleClient<TUserRole>();
        UserRoles.Context = this;

        RoleClaims = new SimpleClient<TRoleClaim>();
        RoleClaims.Context = this;
    }

    public IdentitySqlSugarScope(ConnectionConfig config, Action<SqlSugarClient> action) : base(config, action)
    {
        Roles = new SimpleClient<TRole>();
        Roles.Context = this;

        UserRoles = new SimpleClient<TUserRole>();
        UserRoles.Context = this;

        RoleClaims = new SimpleClient<TRoleClaim>();
        RoleClaims.Context = this;
    }

    public IdentitySqlSugarScope(List<ConnectionConfig> config, Action<SqlSugarClient> action) : base(config, action)
    {
        Roles = new SimpleClient<TRole>();
        Roles.Context = this;

        UserRoles = new SimpleClient<TUserRole>();
        UserRoles.Context = this;

        RoleClaims = new SimpleClient<TRoleClaim>();
        RoleClaims.Context = this;
    }

    public virtual SimpleClient<TRole> Roles { get; set; }
    public virtual SimpleClient<TUserRole> UserRoles { get; set; }
    public virtual SimpleClient<TRoleClaim> RoleClaims { get; set; }
}