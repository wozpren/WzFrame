using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WzFrame.Entity;
using WzFrame.Shared.ApiResult;
using WzFrame.Shared.Services;

namespace WzFrame.Shared.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LicenseController: ControllerBase
    {
        private readonly EntityService<LicenseModel> entityService;

        public LicenseController(EntityService<LicenseModel> entityService)
        {
            this.entityService = entityService;
        }



        [HttpGet]
        public async Task<IActionResult> VaildDevice(string iemi, string type)
        {

            var lincese = await entityService.entityRepository
                .AsQueryable()
                .Where(x => x.Iemi == iemi && x.LicenseType == type)
                .FirstAsync();
            if (lincese != null)
            {
                return Ok(new TResult<bool>(lincese.IsVaild));
            }
            else
            {
                var i = await entityService.entityRepository
                .AsQueryable()
                .Where(x => x.Iemi == iemi && x.LicenseType == type)
                .CountAsync();

                await entityService.AddAsync(new LicenseModel()
                {
                    Iemi = iemi,
                    LicenseType = type,
                    IsVaild = i <= 3
                });

                return Ok(new TResult<bool>(i <= 3));

            }



        }

    }
}
