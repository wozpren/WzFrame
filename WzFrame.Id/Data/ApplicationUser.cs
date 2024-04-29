using Microsoft.AspNetCore.Identity;
using SqlSugar;

namespace WzFrame.Id.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        [SugarColumn(IsPrimaryKey = true)]
        public override string Id { get => base.Id; set => base.Id = value; }

        [SqlSugar.SugarColumn(IsEnableUpdateVersionValidation = true)]
        public override string? ConcurrencyStamp { get => base.ConcurrencyStamp; set => base.ConcurrencyStamp = value; }
    }

}
