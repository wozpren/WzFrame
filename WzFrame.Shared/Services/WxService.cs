using BaiduBce.Services.Bos.Model;
using CommunityToolkit.HighPerformance.Helpers;
using Masuit.Tools.Core.AspNetCore;
using Masuit.Tools.Hardware;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WzFrame.Entity.Workshop;
using WzFrame.Shared.Hubs;
using Mapster;
using WzFrame.Entity;
using WzFrame.Entity.Configuration;
using Microsoft.AspNetCore.Http.HttpResults;
using WzFrame.Shared.ApiResult;

namespace WzFrame.Shared.Services
{
    public class WxService
    {
        public int DesktopCount
        {
            get => desktopCount;
            set
            {
                DeskInfos = new DeskInfo[value];
                for (int i = 0; i < value; i++)
                {
                    DeskInfos[i] = new DeskInfo();
                }
                desktopCount = value;
            }
        }

        private int desktopCount;
        private readonly BlazorHub hubContext;
        private readonly IServiceProvider serviceProvider;


        private Dictionary<WxUser, int> TodayList = new Dictionary<WxUser, int>();
        public DeskInfo[] DeskInfos { get; set; } = Array.Empty<DeskInfo>();




        public WxService(BlazorHub hubContext, IServiceProvider serviceProvider)
        {
            DesktopCount = 10;
            this.hubContext = hubContext;
            this.serviceProvider = serviceProvider;
        }

        public WxUser? FindUserById(int index, int deskId)
        {
            if (index < 0 || index >= DesktopCount)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }
            if (deskId < 0 || deskId > 3)
            {
                throw new ArgumentOutOfRangeException(nameof(deskId));
            }

            if (DeskInfos[index] == null)
            {
                return null;
            }

            if (deskId == 0)
            {
                return DeskInfos[index].User1;
            }
            else if (deskId == 1)
            {
                return DeskInfos[index].User2;
            }
            else if (deskId == 2)
            {
                return DeskInfos[index].User3;
            }
            else if (deskId == 3)
            {
                return DeskInfos[index].User4;
            }
            return null;

        }

        public void Down(int index, int deskId)
        {
            if (index < 0 || index >= DesktopCount)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }
            if (deskId < 0 || deskId > 3)
            {
                throw new ArgumentOutOfRangeException(nameof(deskId));
            }

            if (DeskInfos[index] == null)
            {
                DeskInfos[index] = new DeskInfo();
            }


            if (deskId == 0)
            {
                DeskInfos[index].User1 = new WxUser();
            }
            else if (deskId == 1)
            {
                DeskInfos[index].User2 = new WxUser();
            }
            else if (deskId == 2)
            {
                DeskInfos[index].User3 = new WxUser();
            }
            else if (deskId == 3)
            {
                DeskInfos[index].User4 = new WxUser();
            }

            hubContext.Update();
        }

        public void UpdateDeskInfo(int index, int deskId, WxUser user)
        {
            if (index < 0 || index >= DesktopCount)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }
            if (deskId < 0 || deskId > 3)
            {
                throw new ArgumentOutOfRangeException(nameof(deskId));
            }

            if (DeskInfos[index] == null)
            {
                DeskInfos[index] = new DeskInfo();
            }


            if (deskId == 0)
            {
                DeskInfos[index].User1 = user;
            }
            else if (deskId == 1)
            {
                DeskInfos[index].User2 = user;
            }
            else if (deskId == 2)
            {
                DeskInfos[index].User3 = user;
            }
            else if (deskId == 3)
            {
                DeskInfos[index].User4 = user;
            }

            hubContext.Update();

        }

        public async Task UpdateUserScoreAsync(string id, int score)
        {
            var wxEntityService = GetEntityService<WxUser>();
            var user = await wxEntityService.GetAsync(long.Parse(id));
            user.TotalScore += score;
            UpdateTodayList(user, score);
            await wxEntityService.UpdateAsync(user);
        }


        public async Task UpdateUserScoreAsync(WxUser user, int score)
        {
            user.TotalScore += score;
            UpdateTodayList(user, score);
            var wxEntityService = GetEntityService<WxUser>();
            await wxEntityService.UpdateAsync(user);
        }

        private EntityService<T> GetEntityService<T>() where T : class, IEntityBase, new()
        {
            using var scope = serviceProvider.CreateScope();
            return scope.ServiceProvider.GetRequiredService<EntityService<T>>();
        }

        public void UpdateTodayList(WxUser user, int score)
        {
            if (TodayList.ContainsKey(user))
            {
                TodayList[user] += score;
            }
            else
            {
                TodayList.Add(user, score);
            }
        }

        public List<MWxUser> GetTodayList()
        {
            return TodayList
                .Select(x => new MWxUser() { NickName = x.Key.NickName, Score = x.Value })
                .OrderByDescending(x => x.Score)
                .ToList();

        }

        public string GetBattleMsg()
        {
            return AppSettings.Get<WebsiteConfig>().BattleMsg;
        }




        public  async Task<bool> Order(AppointmentDto dto)
        {
            var getentity = GetEntityService<WxUser>();
            var user = await getentity.GetAsync(dto.UserId);
            if (user == null)
            {
                return false;
            }

            var appoint = dto.Adapt<Appointment>();
            appoint.Name = user.NickName;
            appoint.Phone = user.Phone;

            var config = AppSettings.Get<WebsiteConfig>();

            if(config.OrderList.Length < dto.IndexId)
            {
                var al = GetEntityService<AppointList>();
                var appointment = GetEntityService<Appointment>();
                var count = await al.GetAsync(dto.AppointmentId + dto.IndexId);
                if(count == null)
                {
                    count = new AppointList() { Id = dto.AppointmentId + dto.IndexId, Count = 1 };
                    await al.AddAsync(count);
                    await appointment.AddAsync(appoint);
                    return true;
                }
                else
                {
                    if (count.Count > config.OrderList[dto.IndexId])
                    {
                        return false;
                    }
                    else
                    {
                        count.Count++;
                        await al.UpdateAsync(count);
                        await appointment.AddAsync(appoint);
                        return true;
                    }
                }
            }
            else
            {
                return false;
            }

        }
    }
}
