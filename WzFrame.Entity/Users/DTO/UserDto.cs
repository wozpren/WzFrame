using WzFrame.Entity.Attributes;

namespace WzFrame.Entity.Users.DTO
{
    [DoNotCreateTable]
    [SugarTable("user")]
    public class UserMessage
    {
        public long Id { get; set; }

        public string UserName { get; set; } = string.Empty;

        public string Avatar { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;


    }
}
