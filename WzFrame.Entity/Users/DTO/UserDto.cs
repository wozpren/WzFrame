using WzFrame.Entity.Attributes;
using WzFrame.Entity.Users;

namespace WzFrame.Entity.DTO
{
    [DoNotCreateTable]
    [SugarTable("user")]
    public class UserDTO
    {
        public long Id { get; set; }

        public string UserName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;

        public string Avatar { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public List<RoleMessage>? Roles { get; set; }

    }
}
