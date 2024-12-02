using BaiduBce.Services.Bos.Model;
using Masuit.Tools.Core.AspNetCore;
using Masuit.Tools.Hardware;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WzFrame.Entity.Workshop;
using WzFrame.Shared.Hubs;

namespace WzFrame.Shared.Services
{
    [ServiceInject(ServiceLifetime.Singleton)]
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

        public DeskInfo[] DeskInfos { get; set; } = Array.Empty<DeskInfo>();

        public WxService(BlazorHub hubContext)
        {
            DesktopCount = 10;
            this.hubContext = hubContext;
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

            if(DeskInfos[index] == null)
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





    }
}
