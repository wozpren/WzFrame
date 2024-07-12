using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WzFrame.Shared.Services;

namespace WzFrame.Shared.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EshopController : ControllerBase
    {

        private readonly EShopServics eShopServics;

        public EshopController(EShopServics eShopServics)
        {
            this.eShopServics = eShopServics;
        }

        [HttpGet("getallgoods")]
        public async Task<ActionResult> GetAllGoods(int page)
        {
            return Ok(await eShopServics.GetAllGoods(page));
        }
    }
}
