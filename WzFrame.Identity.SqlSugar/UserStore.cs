using Microsoft.AspNetCore.Identity;
using NetTaste;
using SqlSugar;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using System.Threading;

namespace WzFrame.Identity.SqlSugar
{
    /// <summary>
    /// Represents a new instance of a persistence store for users, using the default implementation
    /// of <see cref="IdentityUser{TKey}"/> with a string as a primary key.
    /// </summary>
    public class UserStore : UserStore<IdentityUser<string>>
    {
        /// <summary>
        /// Constructs a new instance of <see cref="UserStore"/>.
        /// </summary>
        /// <param name="context">The <see cref="DbContext"/>.</param>
        /// <param name="describer">The <see cref="IdentityErrorDescriber"/>.</param>
        public UserStore(ISqlSugarClient context, IdentityErrorDescriber? describer = null) : base(context, describer) { }
    }


    /// <summary>
    /// Creates a new instance of a persistence store for the specified user type.
    /// </summary>
    /// <typeparam name="TUser">The type representing a user.</typeparam>
    public class UserStore<TUser> : UserStore<TUser, IdentityRole, ISqlSugarClient, string>
        where TUser : IdentityUser<string>, new()
    {
        /// <summary>
        /// Constructs a new instance of <see cref="UserStore{TUser}"/>.
        /// </summary>
        /// <param name="context">The <see cref="DbContext"/>.</param>
        /// <param name="describer">The <see cref="IdentityErrorDescriber"/>.</param>
        public UserStore(ISqlSugarClient context, IdentityErrorDescriber? describer = null) : base(context, describer) { }
    }


    /// <summary>
    /// Represents a new instance of a persistence store for the specified user and role types.
    /// </summary>
    /// <typeparam name="TUser">The type representing a user.</typeparam>
    /// <typeparam name="TRole">The type representing a role.</typeparam>
    /// <typeparam name="TContext">The type of the data context class used to access the store.</typeparam>
    public class UserStore<TUser, TRole, TContext> : UserStore<TUser, TRole, TContext, string>
        where TUser : IdentityUser<string>, new()
        where TRole : IdentityRole<string>
        where TContext : ISqlSugarClient
    {
        /// <summary>
        /// Constructs a new instance of <see cref="UserStore{TUser, TRole, TContext}"/>.
        /// </summary>
        /// <param name="context">The <see cref="DbContext"/>.</param>
        /// <param name="describer">The <see cref="IdentityErrorDescriber"/>.</param>
        public UserStore(TContext context, IdentityErrorDescriber? describer = null) : base(context, describer) { }
    }

    /// <summary>
    /// Represents a new instance of a persistence store for the specified user and role types.
    /// </summary>
    /// <typeparam name="TUser">The type representing a user.</typeparam>
    /// <typeparam name="TRole">The type representing a role.</typeparam>
    /// <typeparam name="TContext">The type of the data context class used to access the store.</typeparam>
    /// <typeparam name="TKey">The type of the primary key for a role.</typeparam>
    public class UserStore<TUser, TRole, TContext, TKey> : UserStore<TUser, TRole, TContext, TKey, IdentityUserClaim<TKey>, IdentityUserRole<TKey>, IdentityUserLogin<TKey>, IdentityUserToken<TKey>, IdentityRoleClaim<TKey>>
    where TUser : IdentityUser<TKey>, new()
    where TRole : IdentityRole<TKey>
    where TContext : ISqlSugarClient
    where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Constructs a new instance of <see cref="UserStore{TUser, TRole, TContext, TKey}"/>.
        /// </summary>
        /// <param name="context">The <see cref="DbContext"/>.</param>
        /// <param name="describer">The <see cref="IdentityErrorDescriber"/>.</param>
        public UserStore(TContext context, IdentityErrorDescriber? describer = null) : base(context, describer) { }
    }


    public class UserStore<TUser, TRole, TContext, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TKey, TUserClaim, TUserRole, TUserLogin, TUserToken, TRoleClaim> :
    UserStoreBaseSuagr<TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TUserToken, TRoleClaim>,
    IProtectedUserStore<TUser>
    where TUser : IdentityUser<TKey>, new()
    where TRole : IdentityRole<TKey>
    where TContext : ISqlSugarClient
    where TKey : IEquatable<TKey>
    where TUserClaim : IdentityUserClaim<TKey>, new()
    where TUserRole : IdentityUserRole<TKey>, new()
    where TUserLogin : IdentityUserLogin<TKey>, new()
    where TUserToken : IdentityUserToken<TKey>, new()
    where TRoleClaim : IdentityRoleClaim<TKey>, new()
    {
        public UserStore(TContext context, IdentityErrorDescriber describer) : base(describer ?? new IdentityErrorDescriber())
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            Context = context;
        }

        /// <summary>
        /// Gets the database context for this store.
        /// </summary>
        public virtual TContext Context { get; private set; }

        public override ISugarQueryable<TUser> Users => Context.Queryable<TUser>();

        private ISugarQueryable<TUserClaim> UserClaims => Context.Queryable<TUserClaim>();
        private ISugarQueryable<TUserLogin> UserLogins => Context.Queryable<TUserLogin>();
        private ISugarQueryable<TRole> Roles => Context.Queryable<TRole>();


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

        public override async Task AddToRoleAsync(TUser user, string normalizedRoleName, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (string.IsNullOrWhiteSpace(normalizedRoleName))
            {
                throw new ArgumentException("值不能为null或空。", nameof(normalizedRoleName));
            }
            var roleEntity = await FindRoleAsync(normalizedRoleName, cancellationToken);
            if (roleEntity == null)
            {
                throw new InvalidOperationException($"角色{normalizedRoleName}不存在。");
            }
            Context.Insertable(CreateUserRole(user, roleEntity)).ExecuteCommand();
        }

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

        public override async Task<IdentityResult> DeleteAsync(TUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            await Context.Deleteable(user)                
                .ExecuteCommandAsync(cancellationToken);
            return IdentityResult.Success;
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

        public override async Task<TUser?> FindByIdAsync(string userId, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            var id = ConvertIdFromString(userId);
            return await Context.Queryable<TUser>()
                .InSingleAsync(id);
        }

        public override Task<TUser?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            return Users.Where(u => u.NormalizedUserName == normalizedUserName).FirstAsync(cancellationToken)!;
        }

        public override async Task<IList<Claim>> GetClaimsAsync(TUser user, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var t = await UserClaims
            .Where(uc => uc.UserId.Equals(user.Id))
            .Select(uc => new { uc.ClaimType, uc.ClaimValue })
            .ToListAsync(cancellationToken);
            return t.Select(c => new Claim(c.ClaimType, c.ClaimValue)).ToList();
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
            Context.Ado.CancellationToken = cancellationToken;

            var t = await UserLogins
                .Where(l => l.UserId.Equals(userId))
                .Select(l => new { l.LoginProvider, l.ProviderKey, l.ProviderDisplayName })
                .ToListAsync();

            return t.Select(l => new UserLoginInfo(l.LoginProvider, l.ProviderKey, l.ProviderDisplayName)).ToList();



        }

        public override async Task<IList<string>> GetRolesAsync(TUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            var userId = user.Id;
            Context.Ado.CancellationToken = cancellationToken;

            var query = Roles.InnerJoin<TUserRole>((a, b) => a.Id.Equals(b.RoleId) && b.UserId.Equals(userId));
            return await query.Select(a => a.Name).ToListAsync();
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

            return await query.ToListAsync(cancellationToken);
        }

        public override async Task<IList<TUser>> GetUsersInRoleAsync(string normalizedRoleName, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (string.IsNullOrEmpty(normalizedRoleName))
            {
                throw new ArgumentNullException(nameof(normalizedRoleName));
            }

            var role = await FindRoleAsync(normalizedRoleName, cancellationToken);

            if (role != null)
            {
                var query = Users.InnerJoin<TUserRole>((user, userrole) => user.Id.Equals(userrole.UserId) && userrole.RoleId.Equals(role.Id));

                Context.Ado.CancellationToken = cancellationToken;
                return await query.ToListAsync();
            }
            return new List<TUser>();
        }

        public override async Task<bool> IsInRoleAsync(TUser user, string normalizedRoleName, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (string.IsNullOrWhiteSpace(normalizedRoleName))
            {
                throw new ArgumentException("角色不能为空", nameof(normalizedRoleName));
            }
            var role = await FindRoleAsync(normalizedRoleName, cancellationToken);
            if (role != null)
            {
                var userRole = await FindUserRoleAsync(user.Id, role.Id, cancellationToken);
                return userRole != null;
            }
            return false;
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

        public override async Task RemoveFromRoleAsync(TUser user, string normalizedRoleName, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (string.IsNullOrWhiteSpace(normalizedRoleName))
            {
                throw new ArgumentException("角色不能为空", nameof(normalizedRoleName));
            }
            var roleEntity = await FindRoleAsync(normalizedRoleName, cancellationToken);
            if (roleEntity != null)
            {
                var userRole = await FindUserRoleAsync(user.Id, roleEntity.Id, cancellationToken);
                if (userRole != null)
                {
                    await Context.Deleteable(userRole).ExecuteCommandAsync();
                }
            }
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

        protected override Task AddUserTokenAsync(TUserToken token)
        {
            if (token == null)
            {
                throw new ArgumentNullException(nameof(token));
            }

            Context.Insertable(token).ExecuteCommand();
            return Task.CompletedTask;
        }

        protected override Task<TRole?> FindRoleAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            return Roles.Where(r => r.NormalizedName == normalizedRoleName).FirstAsync(cancellationToken);
        }

        protected override Task<TUserToken?> FindTokenAsync(TUser user, string loginProvider, string name, CancellationToken cancellationToken)
        {
            return Context.Queryable<TUserToken>().Where(t => t.UserId.Equals(user.Id) && t.LoginProvider == loginProvider && t.Name == name).FirstAsync(cancellationToken);
        }

        protected override Task<TUser?> FindUserAsync(TKey userId, CancellationToken cancellationToken)
        {
            Context.Ado.CancellationToken = cancellationToken;
            return Users.SingleAsync(u => u.Id.Equals(userId));
        }

        protected override Task<TUserLogin?> FindUserLoginAsync(TKey userId, string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            Context.Ado.CancellationToken = cancellationToken;
            return UserLogins.SingleAsync(expression: l => l.UserId.Equals(userId) && l.LoginProvider == loginProvider && l.ProviderKey == providerKey);
        }

        protected override Task<TUserLogin?> FindUserLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            Context.Ado.CancellationToken = cancellationToken;
            return UserLogins.SingleAsync(userLogin => userLogin.LoginProvider == loginProvider && userLogin.ProviderKey == providerKey);
        }

        protected override Task<TUserRole?> FindUserRoleAsync(TKey userId, TKey roleId, CancellationToken cancellationToken)
        {
            return Context.Queryable<TUserRole>().Where(ur => ur.UserId.Equals(userId) && ur.RoleId.Equals(roleId)).FirstAsync(cancellationToken);
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
    }
}
