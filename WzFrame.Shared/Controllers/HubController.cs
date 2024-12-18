using Mapster;
using Microsoft.AspNetCore.Mvc;
using WzFrame.Entity;
using WzFrame.Shared.ApiResult;
using WzFrame.Shared.Services;

namespace WzFrame.Shared.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HubController : ControllerBase
    {
        private readonly EntityService<HubUser> entityService;

        public class PostData
        {
            public string Name { get; set; } = string.Empty;
            public string Phone { get; set; } = string.Empty;
            public string MachineCode { get; set; } = string.Empty;
        }

        public HubController(EntityService<HubUser> entityService)
        {
            this.entityService = entityService;
        }


        [HttpPost("Register")]
        public async Task<ActionResult> Register(PostData user)
        {
            HubUser hubUser = user.Adapt<HubUser>();
            await entityService.AddAsync(hubUser);
            return Ok(new JResult(true));
        }
    }
}
