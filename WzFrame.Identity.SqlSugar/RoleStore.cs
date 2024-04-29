using Microsoft.AspNetCore.Identity;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace WzFrame.Identity.SqlSugar;

/// <summary>
/// Creates a new instance of a persistence store for roles.
/// </summary>
/// <typeparam name="TRole">The type of the class representing a role</typeparam>
public class RoleStore<TRole> : RoleStore<TRole, ISqlSugarClient, string>
    where TRole : IdentityRole<string>, new()
{
    /// <summary>
    /// Constructs a new instance of <see cref="RoleStore{TRole}"/>.
    /// </summary>
    /// <param name="context">The <see cref="DbContext"/>.</param>
    /// <param name="describer">The <see cref="IdentityErrorDescriber"/>.</param>
    public RoleStore(ISqlSugarClient context, IdentityErrorDescriber? describer = null) : base(context, describer) { }
}

/// <summary>
/// Creates a new instance of a persistence store for roles.
/// </summary>
/// <typeparam name="TRole">The type of the class representing a role.</typeparam>
/// <typeparam name="TContext">The type of the data context class used to access the store.</typeparam>
public class RoleStore<TRole, TContext> : RoleStore<TRole, TContext, string>
    where TRole : IdentityRole<string>, new()
    where TContext : ISqlSugarClient
{
    /// <summary>
    /// Constructs a new instance of <see cref="RoleStore{TRole, TContext}"/>.
    /// </summary>
    /// <param name="context">The <see cref="DbContext"/>.</param>
    /// <param name="describer">The <see cref="IdentityErrorDescriber"/>.</param>
    public RoleStore(TContext context, IdentityErrorDescriber? describer = null) : base(context, describer) { }
}


/// <summary>
/// Creates a new instance of a persistence store for roles.
/// </summary>
/// <typeparam name="TRole">The type of the class representing a role.</typeparam>
/// <typeparam name="TContext">The type of the data context class used to access the store.</typeparam>
/// <typeparam name="TKey">The type of the primary key for a role.</typeparam>
public class RoleStore<TRole, TContext, TKey> : RoleStore<TRole, TContext, TKey, IdentityUserRole<TKey>, IdentityRoleClaim<TKey>>,
    ISugarQueryableRoleStore<TRole>,
    IRoleClaimStore<TRole>
    where TRole : IdentityRole<TKey>, new()
    where TKey : IEquatable<TKey>
    where TContext : ISqlSugarClient
{
    /// <summary>
    /// Constructs a new instance of <see cref="RoleStore{TRole, TContext, TKey}"/>.
    /// </summary>
    /// <param name="context">The <see cref="DbContext"/>.</param>
    /// <param name="describer">The <see cref="IdentityErrorDescriber"/>.</param>
    public RoleStore(TContext context, IdentityErrorDescriber? describer = null) : base(context, describer) { }
}

/// <summary>
/// Creates a new instance of a persistence store for roles.
/// </summary>
/// <typeparam name="TRole">The type of the class representing a role.</typeparam>
/// <typeparam name="TContext">The type of the data context class used to access the store.</typeparam>
/// <typeparam name="TKey">The type of the primary key for a role.</typeparam>
/// <typeparam name="TUserRole">The type of the class representing a user role.</typeparam>
/// <typeparam name="TRoleClaim">The type of the class representing a role claim.</typeparam>
public class RoleStore<TRole, TContext, TKey, TUserRole, TRoleClaim> :
    ISugarQueryableRoleStore<TRole>,
    IRoleClaimStore<TRole>
    where TRole : IdentityRole<TKey>, new()
    where TKey : IEquatable<TKey>
    where TContext : ISqlSugarClient
    where TUserRole : IdentityUserRole<TKey>, new()
    where TRoleClaim : IdentityRoleClaim<TKey>, new()
{
    public ISugarQueryable<TRole> Users => throw new NotImplementedException();

    /// <summary>
    /// Gets the database context for this store.
    /// </summary>
    public virtual TContext Context { get; private set; }

    /// <summary>
    /// Gets or sets the <see cref="IdentityErrorDescriber"/> for any error that occurred with the current operation.
    /// </summary>
    public IdentityErrorDescriber ErrorDescriber { get; set; }

    /// <summary>
    /// Constructs a new instance of <see cref="RoleStore{TRole, TContext, TKey, TUserRole, TRoleClaim}"/>.
    /// </summary>
    /// <param name="context">The <see cref="DbContext"/>.</param>
    /// <param name="describer">The <see cref="IdentityErrorDescriber"/>.</param>
    public RoleStore(TContext context, IdentityErrorDescriber? describer = null)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }
        Context = context;
        ErrorDescriber = describer ?? new IdentityErrorDescriber();
    }

    private bool _disposed;


    /// <summary>
    /// Throws if this class has been disposed.
    /// </summary>
    protected void ThrowIfDisposed()
    {
        if (_disposed)
        {
            throw new ObjectDisposedException(GetType().Name);
        }
    }
    public virtual TKey ConvertIdFromString(string id)
    {
        if (id == null)
        {
            return default;
        }
        return (TKey)TypeDescriptor.GetConverter(typeof(TKey)).ConvertFromInvariantString(id);
    }

    public virtual string ConvertIdToString(TKey id)
    {
        if (id.Equals(default))
        {
            return null;
        }
        return id.ToString();
    }


    public virtual Task AddClaimAsync(TRole role, Claim claim, CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        if (role == null)
        {
            throw new ArgumentNullException(nameof(role));
        }
        if (claim == null)
        {
            throw new ArgumentNullException(nameof(claim));
        }

        Context.Insertable(CreateRoleClaim(role, claim)).ExecuteCommand();
        return Task.FromResult(false);
    }

    public virtual async Task<IdentityResult> CreateAsync(TRole role, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        if (role == null)
        {
            throw new ArgumentNullException(nameof(role));
        }
        Context.Ado.CancellationToken = cancellationToken;
        await Context.Insertable(role).ExecuteReturnSnowflakeIdAsync();
        return IdentityResult.Success;
    }

    public virtual async Task<IdentityResult> DeleteAsync(TRole role, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        if (role == null)
        {
            throw new ArgumentNullException(nameof(role));
        }
        await Context.Deleteable(role).ExecuteCommandAsync(cancellationToken);
        return IdentityResult.Success;
    }

    public void Dispose() => _disposed = true;

    public Task<TRole?> FindByIdAsync(string id, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        var roleId = ConvertIdFromString(id);
        return Context.Queryable<TRole>().Where(u => u.Id.Equals(roleId)).FirstAsync(cancellationToken)!;
    }

    public virtual Task<TRole?> FindByNameAsync(string normalizedName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        return Context.Queryable<TRole>().Where(r => r.NormalizedName == normalizedName).FirstAsync(cancellationToken)!;
    }

    public virtual async Task<IList<Claim>> GetClaimsAsync(TRole role, CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        if (role == null)
        {
            throw new ArgumentNullException(nameof(role));
        }

        var t = await Context.Queryable<TRoleClaim>()
            .Where(rc => rc.RoleId.Equals(role.Id))
            .Select(c => new { c.ClaimType, c.ClaimValue })
            .ToListAsync(cancellationToken);
        return t.Select(c => new Claim(c.ClaimType, c.ClaimValue)).ToList();
    }

    public Task<string?> GetNormalizedRoleNameAsync(TRole role, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        if (role == null)
        {
            throw new ArgumentNullException(nameof(role));
        }
        return Task.FromResult(role.NormalizedName);
    }


    public virtual Task<string> GetRoleIdAsync(TRole role, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        if (role == null)
        {
            throw new ArgumentNullException(nameof(role));
        }
        return Task.FromResult(ConvertIdToString(role.Id));
    }

    public Task<string?> GetRoleNameAsync(TRole role, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        if (role == null)
        {
            throw new ArgumentNullException(nameof(role));
        }
        return Task.FromResult(role.Name);
    }

    public virtual async Task RemoveClaimAsync(TRole role, Claim claim, CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        if (role == null)
        {
            throw new ArgumentNullException(nameof(role));
        }
        if (claim == null)
        {
            throw new ArgumentNullException(nameof(claim));
        }
        var claims = await Context.Queryable<TRoleClaim>()
            .Where(rc => rc.RoleId.Equals(role.Id) && rc.ClaimValue == claim.Value && rc.ClaimType == claim.Type)
            .ToListAsync(cancellationToken);

        await Context.Deleteable(claims).ExecuteCommandAsync(cancellationToken);
    }

    public Task SetNormalizedRoleNameAsync(TRole role, string? normalizedName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        if (role == null)
        {
            throw new ArgumentNullException(nameof(role));
        }
        role.NormalizedName = normalizedName;
        //await Context.Updateable(role).ExecuteCommandAsync(cancellationToken);
        return Task.CompletedTask;
    }

    public Task SetRoleNameAsync(TRole role, string? roleName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        if (role == null)
        {
            throw new ArgumentNullException(nameof(role));
        }
        role.Name = roleName;
        //await Context.Updateable(role).ExecuteCommandAsync(cancellationToken);
        return Task.CompletedTask;
    }

    public async Task<IdentityResult> UpdateAsync(TRole role, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        if (role == null)
        {
            throw new ArgumentNullException(nameof(role));
        }
        //Context.Attach(role);
        //role.ConcurrencyStamp = Guid.NewGuid().ToString();
        
        try
        {
            await Context.Updateable(role).ExecuteCommandAsync(cancellationToken);
        }
        catch (VersionExceptions)
        {
            return IdentityResult.Failed(ErrorDescriber.ConcurrencyFailure());
        }
        return IdentityResult.Success;
    }


    protected virtual TRoleClaim CreateRoleClaim(TRole role, Claim claim)
    => new TRoleClaim { RoleId = role.Id, ClaimType = claim.Type, ClaimValue = claim.Value };
}