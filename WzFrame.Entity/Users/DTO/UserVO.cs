using WzFrame.Entity.Attributes;
using WzFrame.Entity.Users;

namespace WzFrame.Entity.DTO
{
    [DoNotCreateTable]
    [SugarTable("user")]
    public class UserVO
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public long Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Avatar { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Belong { get; set; } = string.Empty;

        [Navigate(typeof(IdentityUserRole), nameof(IdentityUserRole.UserId), nameof(IdentityUserRole.RoleId))]
        public List<RoleDto>? Roles { get; set; }


        public bool ContainsRole(string roleName)
        {
            if (Roles == null)
            {
                return false;
            }

            return Roles.Any(r => r.Name == roleName);
        }


    }
}
