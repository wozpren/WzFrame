using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WzFrame.Entity.Work;
using WzFrame.Shared.ApiResult;
using WzFrame.Shared.Services;

namespace WzFrame.Shared.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WorkerController : ControllerBase
    {
        private readonly EntityService<Worker> entityService;
        private readonly EntityService<WorkerP> entityServicev2;

        public WorkerController(EntityService<Worker> entityService, EntityService<WorkerP> entityServicev2)
        {
            this.entityService = entityService;
            this.entityServicev2 = entityServicev2;
        }

        [HttpGet("getv2")]
        public async Task<IActionResult> GetV2(long id)
        {
            var data = await entityServicev2.GetAsync(id);
            return Ok(new Result<WorkerP>(data));
        }


        [HttpGet("get")]
        public async Task<IActionResult> Get(long id)
        {
            var data = await entityService.GetAsync(id);
            return Ok(new Result<Worker>(data));
        }

        [HttpOptions("get")]
        public IActionResult CheckGet(long id)
        {
            return NoContent();
        }
    }
}
