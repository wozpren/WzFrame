using Microsoft.AspNetCore.Identity;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace WzFrame.Identity.SqlSugar;

/// <summary>
/// Creates a new instance of a persistence store for the specified user type.
/// </summary>
/// <typeparam name="TUser">The type representing a user.</typeparam>
public class UserOnlyStore<TUser> : UserOnlyStore<TUser, ISqlSugarClient, string> where TUser : IdentityUser<string>, new()
{
    /// <summary>
    /// Constructs a new instance of <see cref="UserOnlyStore{TUser}"/>.
    /// </summary>
    /// <param name="context">The <see cref="DbContext"/>.</param>
    /// <param name="describer">The <see cref="IdentityErrorDescriber"/>.</param>
    public UserOnlyStore(ISqlSugarClient context, IdentityErrorDescriber? describer = null) : base(context, describer) { }
}

/// <summary>
/// Represents a new instance of a persistence store for the specified user and role types.
/// </summary>
/// <typeparam name="TUser">The type representing a user.</typeparam>
/// <typeparam name="TContext">The type of the data context class used to access the store.</typeparam>
public class UserOnlyStore<TUser, TContext> : UserOnlyStore<TUser, TContext, string>
    where TUser : IdentityUser<string>, new()
    where TContext : ISqlSugarClient
{
    /// <summary>
    /// Constructs a new instance of <see cref="UserStore{TUser, TRole, TContext}"/>.
    /// </summary>
    /// <param name="context">The <see cref="DbContext"/>.</param>
    /// <param name="describer">The <see cref="IdentityErrorDescriber"/>.</param>
    public UserOnlyStore(TContext context, IdentityErrorDescriber? describer = null) : base(context, describer) { }
}

/// <summary>
/// Represents a new instance of a persistence store for the specified user and role types.
/// </summary>
/// <typeparam name="TUser">The type representing a user.</typeparam>
/// <typeparam name="TContext">The type of the data context class used to access the store.</typeparam>
/// <typeparam name="TKey">The type of the primary key for a role.</typeparam>
public class UserOnlyStore<TUser, TContext, TKey> : UserOnlyStore<TUser, TContext, TKey, IdentityUserClaim<TKey>, IdentityUserLogin<TKey>, IdentityUserToken<TKey>>
    where TUser : IdentityUser<TKey>, new()
    where TContext : ISqlSugarClient
    where TKey : IEquatable<TKey>
{
    /// <summary>
    /// Constructs a new instance of <see cref="UserStore{TUser, TRole, TContext, TKey}"/>.
    /// </summary>
    /// <param name="context">The <see cref="DbContext"/>.</param>
    /// <param name="describer">The <see cref="IdentityErrorDescriber"/>.</param>
    public UserOnlyStore(TContext context, IdentityErrorDescriber? describer = null) : base(context, describer) { }
}

/// <summary>
/// Represents a new instance of a persistence store for the specified user and role types.
/// </summary>
/// <typeparam name="TUser">The type representing a user.</typeparam>
/// <typeparam name="TContext">The type of the data context class used to access the store.</typeparam>
/// <typeparam name="TKey">The type of the primary key for a role.</typeparam>
/// <typeparam name="TUserClaim">The type representing a claim.</typeparam>
/// <typeparam name="TUserLogin">The type representing a user external login.</typeparam>
/// <typeparam name="TUserToken">The type representing a user token.</typeparam>
public class UserOnlyStore<TUser, TContext, TKey, TUserClaim, TUserLogin, TUserToken> :
    UserStoreBaseSuagr<TUser, TKey, TUserClaim, TUserLogin, TUserToken>,
    IUserLoginStore<TUser>,
    IUserClaimStore<TUser>,
    IUserPasswordStore<TUser>,
    IUserSecurityStampStore<TUser>,
    IUserEmailStore<TUser>,
    IUserLockoutStore<TUser>,
    IUserPhoneNumberStore<TUser>,
    ISugarQueryableUserStore<TUser>,
    IUserTwoFactorStore<TUser>,
    IUserAuthenticationTokenStore<TUser>,
    IUserAuthenticatorKeyStore<TUser>,
    IUserTwoFactorRecoveryCodeStore<TUser>,
    IProtectedUserStore<TUser>
    where TUser : IdentityUser<TKey>, new()
    where TContext : ISqlSugarClient
    where TKey : IEquatable<TKey>
    where TUserClaim : IdentityUserClaim<TKey>, new()
    where TUserLogin : IdentityUserLogin<TKey>, new()
    where TUserToken : IdentityUserToken<TKey>, new()
{

    public virtual TContext Context { get; private set; }

    public override ISugarQueryable<TUser> Users => Context.Queryable<TUser>();
    private ISugarQueryable<TUserClaim> UserClaims => Context.Queryable<TUserClaim>();
    private ISugarQueryable<TUserLogin> UserLogins => Context.Queryable<TUserLogin>();

    /// <summary>
    /// Creates a new instance of the store.
    /// </summary>
    /// <param name="context">The context used to access the store.</param>
    /// <param name="describer">The <see cref="IdentityErrorDescriber"/> used to describe store errors.</param>
    public UserOnlyStore(TContext context, IdentityErrorDescriber? describer = null) : base(describer ?? new IdentityErrorDescriber())
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }
        Context = context;
    }

    /// <summary>
    /// Creates the specified <paramref name="user"/> in the user store.
    /// </summary>
    /// <param name="user">The user to create.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="IdentityResult"/> of the creation operation.</returns>
    public override async Task<IdentityResult> CreateAsync(TUser user, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }
        Context.Ado.CancellationToken = cancellationToken;
        await Context.Insertable(user).ExecuteReturnSnowflakeIdAsync();
        return IdentityResult.Success;
    }

    public override async Task<IdentityResult> UpdateAsync(TUser user, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        //Context.Attach(user);
        //user.ConcurrencyStamp = Guid.NewGuid().ToString();
        try
        {
            Context.Ado.CancellationToken = cancellationToken;
            await Context.Updateable(user).ExecuteCommandWithOptLockAsync();
        }
        catch (VersionExceptions)
        {
            return IdentityResult.Failed(ErrorDescriber.ConcurrencyFailure());
        }
        return IdentityResult.Success;
    }

    public override async Task<IdentityResult> DeleteAsync(TUser user, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }
        await Context.Deleteable(user).ExecuteCommandAsync(cancellationToken);
        return IdentityResult.Success;
    }

    public override async Task<TUser?> FindByIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        var id = ConvertIdFromString(userId);
        return await Context.Queryable<TUser>().InSingleAsync(id);
    }

    public override async Task<TUser?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();

        var t = await Users.Where(u => u.NormalizedUserName == normalizedUserName).FirstAsync(cancellationToken)!;
        return t;
    }

    protected override Task<TUser> FindUserAsync(TKey userId, CancellationToken cancellationToken)
    {
        Context.Ado.CancellationToken = cancellationToken;
        return Users.SingleAsync(u => u.Id.Equals(userId));
    }

    protected override Task<TUserLogin> FindUserLoginAsync(TKey userId, string loginProvider, string providerKey, CancellationToken cancellationToken)
    {
        Context.Ado.CancellationToken = cancellationToken;
        return Context.Queryable<TUserLogin>().SingleAsync(l => l.UserId.Equals(userId) && l.LoginProvider == loginProvider && l.ProviderKey == providerKey);
    }

    protected override Task<TUserLogin> FindUserLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken)
    {
        Context.Ado.CancellationToken = cancellationToken;
        return Context.Queryable<TUserLogin>().SingleAsync(userLogin => userLogin.LoginProvider == loginProvider && userLogin.ProviderKey == providerKey);
    }

    public override async Task<IList<Claim>> GetClaimsAsync(TUser user, CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        Context.Ado.CancellationToken = cancellationToken;
        var tlist = await UserClaims
        .Where(uc => uc.UserId.Equals(user.Id))
        .Select(uc => new { uc.ClaimType, uc.ClaimValue })
        .ToListAsync();
        return tlist.Select(t => new Claim(t.ClaimType, t.ClaimValue)).ToList();
    }

    public override async Task AddClaimsAsync(TUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }
        if (claims == null)
        {
            throw new ArgumentNullException(nameof(claims));
        }
        var list = new List<TUserClaim>();
        foreach (var claim in claims)
        {
            list.Add(CreateUserClaim(user, claim));
        }
        await Context.Insertable(list).ExecuteCommandAsync();
    }

    public override async Task ReplaceClaimAsync(TUser user, Claim claim, Claim newClaim, CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }
        if (claim == null)
        {
            throw new ArgumentNullException(nameof(claim));
        }
        if (newClaim == null)
        {
            throw new ArgumentNullException(nameof(newClaim));
        }
        Context.Ado.CancellationToken = cancellationToken;
        var matchedClaims = await UserClaims.Where(uc => uc.UserId.Equals(user.Id) && uc.ClaimValue == claim.Value && uc.ClaimType == claim.Type).ToListAsync();
        foreach (var matchedClaim in matchedClaims)
        {
            matchedClaim.ClaimValue = newClaim.Value;
            matchedClaim.ClaimType = newClaim.Type;
        }
        await Context.Updateable(matchedClaims).ExecuteCommandWithOptLockAsync(true);
    }

    public override async Task RemoveClaimsAsync(TUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }
        if (claims == null)
        {
            throw new ArgumentNullException(nameof(claims));
        }
        foreach (var claim in claims)
        {
            Context.Ado.CancellationToken = cancellationToken;
            var matchedClaims = await UserClaims.Where(uc => uc.UserId.Equals(user.Id) && uc.ClaimValue == claim.Value && uc.ClaimType == claim.Type).ToListAsync();
            await Context.Deleteable(matchedClaims).ExecuteCommandAsync();
        }
    }

    public override Task AddLoginAsync(TUser user, UserLoginInfo login, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();

        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }
        if (login == null)
        {
            throw new ArgumentNullException(nameof(login));
        }
        Context.Insertable(CreateUserLogin(user, login)).ExecuteCommand();
        return Task.CompletedTask;
    }

    public override async Task RemoveLoginAsync(TUser user, string loginProvider, string providerKey, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }
        var entry = await FindUserLoginAsync(user.Id, loginProvider, providerKey, cancellationToken);
        if (entry != null)
        {
            await Context.Deleteable(entry).ExecuteCommandAsync();
        }
    }

    public override async Task<IList<UserLoginInfo>> GetLoginsAsync(TUser user, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }
        var userId = user.Id;
        var t = await UserLogins
            .Where(l => l.UserId.Equals(userId))
            .Select(l => new { l.LoginProvider, l.ProviderKey, l.ProviderDisplayName })
            .ToListAsync(cancellationToken);
        return t.Select(l => new UserLoginInfo(l.LoginProvider, l.ProviderKey, l.ProviderDisplayName)).ToList();

    }

    public override async Task<TUser?> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();

        var users = await Context.Queryable<TUser>().Where(u => u.NormalizedEmail == normalizedEmail).ToListAsync(cancellationToken);
        if (users.Count > 1)
        {
            throw new InvalidOperationException("存在重复邮箱地址");
        }
        return users.FirstOrDefault();
    }

    public override async Task<IList<TUser>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        if (claim == null)
        {
            throw new ArgumentNullException(nameof(claim));
        }

        var query = Users
            .InnerJoin<TUserClaim>((user, userclaims) => user.Id.Equals(userclaims.UserId) && userclaims.ClaimValue == claim.Value && userclaims.ClaimType == claim.Type);

        Context.Ado.CancellationToken = cancellationToken;
        return await query.ToListAsync();
    }

    protected override Task<TUserToken> FindTokenAsync(TUser user, string loginProvider, string name, CancellationToken cancellationToken)
    {
        return Context.Queryable<TUserToken>().Where(t => t.UserId.Equals(user.Id) && t.LoginProvider == loginProvider && t.Name == name).FirstAsync(cancellationToken);
    }

    protected override Task AddUserTokenAsync(TUserToken token)
    {
        if (token == null)
        {
            throw new ArgumentNullException(nameof(token));
        }

        Context.Insertable(token).ExecuteCommand();
        return Task.CompletedTask;
    }

    protected override Task RemoveUserTokenAsync(TUserToken token)
    {
        if (token == null)
        {
            throw new ArgumentNullException(nameof(token));
        }

        Context.Deleteable(token).ExecuteCommand();
        return Task.CompletedTask;
    }

    public override async Task SetTokenAsync(TUser user, string loginProvider, string name, string? value, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();

        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        var token = await FindTokenAsync(user, loginProvider, name, cancellationToken);
        if (token == null)
        {
            await AddUserTokenAsync(CreateUserToken(user, loginProvider, name, value));
        }
        else
        {
            token.Value = value;
            await Context.Updateable(token).ExecuteCommandAsync();
        }
    }
}
