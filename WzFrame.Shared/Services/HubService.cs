using Masuit.Tools.Core.AspNetCore;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace WzFrame.Shared.Services
{
    [ServiceInject(ServiceLifetime.Singleton)]
    public class HubService
    {
        public ConcurrentDictionary<string, ClaimsPrincipal?> ConnectUser { get; set; } = new ConcurrentDictionary<string, ClaimsPrincipal?>();

        public void AddConnectUser(string userIdentifier, ClaimsPrincipal user)
        {
            ConnectUser.TryAdd(userIdentifier, user);
        }

        public void RemoveConnect(string userId)
        {
            ConnectUser.Remove(userId, out _);
        }


        public IClientProxy? GetClient(IHubClients client, long userId)
        {
            var connectionId = GetConnectionId(userId);
            if (connectionId != null)
            {
                return client.User(connectionId);
            }
            return null;
        }

        public string? GetConnectionId(long userId)
        {
            var user = ConnectUser.FirstOrDefault(x => x.Value.FindFirstValue(ClaimTypes.NameIdentifier) == userId.ToString());
            return user.Key;
        }        
    }
}
