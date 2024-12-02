using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WzFrame.Entity.Workshop
{
    [SugarTable("WxUser")]
    public class WxUser : EntityBase
    {
        public string OpenId { get; set; } = string.Empty;
        public string NickName { get; set; } = string.Empty;

        [SugarColumn(ColumnDataType = "BLOB")]
        public byte[]? Avatar { get; set; }
        public int TotalScore { get; set; }

        [SugarColumn(IsIgnore = true)]
        public string AvatarUrl
        {
            get
            {
                if (Avatar is null)
                    return "/default.png";
                else
                    return "data:image/jpg;base64," + Convert.ToBase64String(Avatar ?? Array.Empty<byte>());
            }
            set
            {
                Avatar = Convert.FromBase64String(value);
            }
        }
    }


    public class LoginRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Avatar { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;

    }
}
