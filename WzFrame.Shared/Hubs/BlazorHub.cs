using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WzFrame.Entity.System;
using WzFrame.Shared.Extensions;
using WzFrame.Shared.Services;

namespace WzFrame.Shared.Hubs
{
    public sealed class BlazorHub : Hub
    {
        private readonly HubService hubService;

        public BlazorHub(HubService hubService)
        {
            this.hubService = hubService;
        }

        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            hubService.ConnectUser.Remove(Context.ConnectionId,  out _);
            return base.OnDisconnectedAsync(exception);
        }

        public Task RefreshUser(string Id)
        {
            return Clients.User(Id)                
                .SendAsync("Refresh");
        }

        public Task SendNotification(string Id, OnlineNotification onlineNotification)
        {
            return Clients.User(Id).SendAsync("ReceiveNotification", onlineNotification);
        }





    }
}
