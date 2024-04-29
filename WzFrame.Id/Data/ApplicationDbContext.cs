using Microsoft.AspNetCore.Identity;
using SqlSugar;
using WzFrame.Identity.SqlSugar;

namespace WzFrame.Id.Data
{
    public class ApplicationDbContext : IdentitySqlSugarScope<ApplicationUser, IdentityRole, string, IdentityUserClaim, IdentityUserRole, IdentityUserLogin, IdentityRoleClaim, IdentityUserToken>
    {
        public ApplicationDbContext(ConnectionConfig config) : base(config)
        {
        }

        public ApplicationDbContext(ConnectionConfig config, Action<SqlSugarClient> action) : base(config, action)
        {            
        }

    }
}
