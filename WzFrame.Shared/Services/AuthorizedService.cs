using Masuit.Tools.Core.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WzFrame.Entity.Users;
using WzFrame.Shared.DataBase;

namespace WzFrame.Shared.Services
{
    [ServiceInject(ServiceLifetime.Scoped)]
    public class AuthorizedService
    {
        private readonly EntityService<ApplicationUser> userService;
        private readonly EntityService<Role> roleService;

        private readonly ApplicationDbContext db;

        public AuthorizedService(EntityService<ApplicationUser> userService, EntityService<Role> roleService, ApplicationDbContext db)
        {
            this.userService = userService;
            this.roleService = roleService;
            this.db = db;
        }



        public Task<bool> LoginAsync(string userName, string password)
        {
            return Task.FromResult(true);

        }

        public void Test()
        {
        }




    }
}
