using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace WzFrame.Shared.Hubs
{
    public class UserIdProvider : IUserIdProvider
    {
        public virtual string GetUserId(HubConnectionContext connection)
        {
            Console.WriteLine(connection.User.Identity?.IsAuthenticated);
            return connection.ConnectionId;
        }
    }
}
