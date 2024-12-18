using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WzFrame.Entity;
using WzFrame.Entity.System;
using WzFrame.Shared.Extensions;
using WzFrame.Shared.Services;

namespace WzFrame.Shared.Hubs
{
    public struct LimitApp
    {
        public string Name;
        public uint Limit;
    }

    public sealed class BlazorHub : Hub
    {
        private readonly HubService hubService;
        private readonly EntityService<HubUser> userEnitty;


        public BlazorHub(HubService hubService, IServiceProvider serviceProvider)
        {
            this.hubService = hubService;
            var scope = serviceProvider.CreateScope();
            userEnitty = scope.ServiceProvider.GetService<EntityService<HubUser>>()!;
            scope.Dispose();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            Console.WriteLine($"{Context.ConnectionId}：断开连接");
            hubService.ConnectUser.Remove(Context.ConnectionId,  out _);

            await userEnitty.entityRepository.AsUpdateable()
                .Where(x => x.ConnectionId == Context.ConnectionId)
                .SetColumns(x => x.IsOnline == false)
                .SetColumns(x => x.LastLogout == DateTime.Now)
                .ExecuteCommandAsync();
        }

        public async void Login(string machineCode)
        {
            hubService.ConnectUser.TryAdd(Context.ConnectionId, default);
            await userEnitty.entityRepository.AsUpdateable()
                .Where(x => x.MachineCode == machineCode)
                .SetColumns(x => x.ConnectionId == Context.ConnectionId)
                .SetColumns(x => x.IsOnline == true)
                .ExecuteCommandAsync();
        }

        public Task RunNetLimit(string id)
        {
            return Clients.User(id).SendAsync("Run");
        }

        public Task PauseNetLimit(string id)
        {
            return Clients.User(id).SendAsync("Pause");
        }


        public Task RefreshUser(string Id)
        {
            return Clients.User(Id)                
                .SendAsync("Refresh");
        }


        public List<LimitApp> GetLimitApps()
        {
            // 这里是你的逻辑代码
            return new List<LimitApp>();
        }

        public Task SendNotification(string Id, OnlineNotification onlineNotification)
        {
            return Clients.User(Id).SendAsync("ReceiveNotification", onlineNotification);
        }

        public Task Update()
        {
            return Clients.All.SendAsync("Update");
        }


        public List<string> GetAllConnectedClients()
        {
            var connectedClients = hubService.ConnectUser.Keys.ToList();
            return connectedClients;
        }


    }
}
