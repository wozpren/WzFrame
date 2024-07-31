using Masuit.Tools.Core.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WzFrame.Entity.DTO;

namespace WzFrame.Shared.Services
{
    [ServiceInject(ServiceLifetime.Scoped)]
    public class WebService
    {
        public UserVO? CurrentUser { get; set; }

        public void ResgisterUser(UserVO user)
        {
            CurrentUser = user;
        }


    }
}
