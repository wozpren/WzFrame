using Microsoft.AspNetCore.Identity;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WzFrame.Identity.SqlSugar;

public abstract class IdentityUserSqlSugarScope<TUser> : IdentityUserSqlSugarScope<TUser, string>
    where TUser : IdentityUser, new()
{
    public IdentityUserSqlSugarScope(ConnectionConfig connectionConfig) : base(connectionConfig)
    {
    }
}

public abstract class IdentityUserSqlSugarScope<TUser, TKey> : IdentityUserSqlSugarScope<TUser, TKey, IdentityUserClaim<TKey>, IdentityUserLogin<TKey>, IdentityUserToken<TKey>>
    where TUser : IdentityUser<TKey>, new()
    where TKey : IEquatable<TKey>
{
    public IdentityUserSqlSugarScope(ConnectionConfig connectionConfig) : base(connectionConfig)
    {
    }
}


public abstract class IdentityUserSqlSugarScope<TUser, TKey, TUserClaim, TUserLogin, TUserToken> : SqlSugarScope
    where TUser : IdentityUser<TKey>, new()
    where TKey : IEquatable<TKey>
    where TUserClaim : IdentityUserClaim<TKey>, new()
    where TUserLogin : IdentityUserLogin<TKey>, new()
    where TUserToken : IdentityUserToken<TKey>, new()
{
    public IdentityUserSqlSugarScope(ConnectionConfig connectionConfig): base(connectionConfig)
    {
        Users = new SimpleClient<TUser>();
        Users.Context = this;

        UserClaims = new SimpleClient<TUserClaim>();
        UserClaims.Context = this;

        UserLogins = new SimpleClient<TUserLogin>();
        UserLogins.Context = this;

        UserTokens = new SimpleClient<TUserToken>();
        UserTokens.Context = this;
    }

    public IdentityUserSqlSugarScope(ConnectionConfig config, Action<SqlSugarClient> action) : base(config, action)
    {
        Users = new SimpleClient<TUser>();
        Users.Context = this;

        UserClaims = new SimpleClient<TUserClaim>();
        UserClaims.Context = this;

        UserLogins = new SimpleClient<TUserLogin>();
        UserLogins.Context = this;

        UserTokens = new SimpleClient<TUserToken>();
        UserTokens.Context = this;
    }

    public IdentityUserSqlSugarScope(List<ConnectionConfig> config, Action<SqlSugarClient> action) : base(config, action)
    {
        Users = new SimpleClient<TUser>();
        Users.Context = this;

        UserClaims = new SimpleClient<TUserClaim>();
        UserClaims.Context = this;

        UserLogins = new SimpleClient<TUserLogin>();
        UserLogins.Context = this;

        UserTokens = new SimpleClient<TUserToken>();
        UserTokens.Context = this;
    }

    public virtual SimpleClient<TUser> Users { get; set; }
    public virtual SimpleClient<TUserClaim> UserClaims { get; set; }
    public virtual SimpleClient<TUserLogin> UserLogins { get; set; }
    public virtual SimpleClient<TUserToken> UserTokens { get; set; }




}