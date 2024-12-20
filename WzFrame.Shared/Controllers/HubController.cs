using Mapster;
using Microsoft.AspNetCore.Mvc;
using WzFrame.Entity.System;
using WzFrame.Shared.ApiResult;
using WzFrame.Shared.Hubs;
using WzFrame.Shared.Services;

namespace WzFrame.Shared.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HubController : ControllerBase
    {
        public class HubDto
        {
            public string MachineCode { get; set; } = "";
            public string Password { get; set; } = "";
        }

        private readonly EntityService<HubUser> entityService;
        private readonly BlazorHub blazorHub;

        public class PostData
        {
            public string Name { get; set; } = string.Empty;
            public string Phone { get; set; } = string.Empty;
            public string MachineCode { get; set; } = string.Empty;
        }

        public HubController(EntityService<HubUser> entityService, BlazorHub blazorHub)
        {
            this.entityService = entityService;
            this.blazorHub = blazorHub;
        }


        [HttpPost("Register")]
        public async Task<ActionResult> Register(PostData user)
        {
            HubUser hubUser = user.Adapt<HubUser>();
            await entityService.AddAsync(hubUser);
            return Ok(new JResult(true));
        }


        [HttpPost("Pause")]
        public async Task<IActionResult> Pause(HubDto hubDto)
        {
            var huser = await entityService.entityRepository
                .AsQueryable()
                .Where(x => x.MachineCode == hubDto.MachineCode)
                .FirstAsync();

            if(huser != null && huser.Password == hubDto.Password && huser.IsOnline)
            {
                await blazorHub.PauseNetLimit(huser.ConnectionId);
                return Ok(new JResult(true));
            }
            return Ok(new JResult(false));
        }


        [HttpPost("Run")]
        public async Task<IActionResult> Run(HubDto hubDto)
        {
            var huser = await entityService.entityRepository
                .AsQueryable()
                .Where(x => x.MachineCode == hubDto.MachineCode)
                .FirstAsync();

            if (huser != null && huser.Password == hubDto.Password && huser.IsOnline)
            {
                await blazorHub.RunNetLimit(huser.ConnectionId);
                return Ok(new JResult(true));
            }
            return Ok(new JResult(false));
        }

    }
}
