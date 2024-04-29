using Masuit.Tools.Core.AspNetCore;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WzFrame.Entity.System;
using WzFrame.Entity.Users;
using WzFrame.Shared.Hubs;
using WzFrame.Shared.Repository;

namespace WzFrame.Shared.Services
{
    [ServiceInject(ServiceLifetime.Scoped)]   
    public class NotificationService : EntityService<UserNotification>
    {
        private readonly IHubContext<BlazorHub> hubContext;
        private readonly HubService hubServer;

        public NotificationService(EntityRepository<UserNotification> entityRepository, IHubContext<BlazorHub> hubContext, HubService hubServer) : base(entityRepository)
        {
            this.hubContext = hubContext;
            this.hubServer = hubServer;
        }

        public async Task<long> AddNotificationAsync(long sendUserId, long receiveUserId, string content, string type = "系统通知")
        {
            var notification = new UserNotification()
            {
                SendUserId = sendUserId,
                ReceiveUserId = receiveUserId,
                Content = content,
                IsRead = false,
                Type = type,
                SendTime = DateTime.Now,               
            };
            var client = hubServer.GetClient(hubContext.Clients,receiveUserId);
            if (client != null)
            {
                var onlinenotification = new OnlineNotification()
                {
                    Category = BootstrapBlazor.Components.ToastCategory.Information,
                    Title = type,
                    Content = content
                };
                await client.SendAsync("ReceiveNotification", onlinenotification);
            }


            return await entityRepository.InsertReturnSnowflakeIdAsync(notification);
        }

        //get unread notification
        public async Task<List<UserNotification>> GetUnreadNotificationAsync(long userId)
        {
            return await entityRepository.AsQueryable()
                .Where(it => it.ReceiveUserId == userId && it.IsRead == false)
                .ToPageListAsync(0, 99);
        }

        //set notifications read
        public async Task<bool> SetNotificationRead(List<long> ids)
        {
            if (ids == null || ids.Count == 0)
            {
                return false;
            }

            return await entityRepository
                .AsUpdateable()
                .SetColumns(it => new UserNotification{ IsRead = true })
                .Where(it => ids.Contains(it.Id))
                .ExecuteCommandHasChangeAsync();
        }


    }
}
