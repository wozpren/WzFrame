using Microsoft.AspNetCore.Identity;
using SqlSugar;
using WzFrame.Entity.Users;
using WzFrame.Identity.SqlSugar;

namespace WzFrame.Shared.DataBase
{
    public class ApplicationDbContext : IdentitySqlSugarScope<ApplicationUser, Role, long, IdentityUserClaim, IdentityUserRole, IdentityUserLogin, IdentityRoleClaim, IdentityUserToken>
    {
        public ApplicationDbContext(ConnectionConfig config) : base(config)
        {
        }

        public ApplicationDbContext(ConnectionConfig config, Action<SqlSugarClient> action) : base(config, action)
        {            
        }

        public ApplicationDbContext(List<ConnectionConfig> config, Action<SqlSugarClient> action) : base(config, action)
        {
        }

    }
}
