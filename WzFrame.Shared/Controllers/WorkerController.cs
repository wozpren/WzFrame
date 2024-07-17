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

        public WorkerController(EntityService<Worker> entityService)
        {
            this.entityService = entityService;
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
