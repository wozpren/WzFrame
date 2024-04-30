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
            if(connection.User?.Identity?.IsAuthenticated == true)
            {
                return connection.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            }
            return connection.ConnectionId;
        }
    }
}
