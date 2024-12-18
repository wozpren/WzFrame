using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Senparc.Weixin.WxOpen.AdvancedAPIs.Sns;
using Senparc.Weixin.WxOpen.Containers;
using Senparc.Weixin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WzFrame.Entity.Workshop;
using WzFrame.Shared.Services;
using WzFrame.Shared.ApiResult;
using COSXML.Network;
using Mapster;
using Senparc.Weixin.WxOpen.AdvancedAPIs.WxApp;

namespace WzFrame.Shared.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WxController : ControllerBase
    {
        private readonly WxService wxService;
        private readonly EntityService<WxUser> wxUserService;
        private readonly EntityService<Appointment> appointUserService;
        private readonly EntityService<AppointList> appointListService;
        private const string WxOpenAppId = "wx3dfbaf4673340e6e";
        private const string WxOpenAppSecret = "533e87ac8c7c96e8ca94f9370fc8b6da";

        public WxController(WxService wxService, EntityService<WxUser> wxUserService, EntityService<Appointment> appointUserService, EntityService<AppointList> appointListService)
        {
            this.wxService = wxService;
            this.wxUserService = wxUserService;
            this.appointUserService = appointUserService;
            this.appointListService = appointListService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest req)
        {
            try
            {
                var jsonResult = SnsApi.JsCode2Json(WxOpenAppId, WxOpenAppSecret, req.Code);
                if (jsonResult.errcode == ReturnCode.请求成功)
                {
                    //Session["WxOpenUser"] = jsonResult;//使用Session保存登陆信息（不推荐）
                    //使用SessionContainer管理登录信息（推荐）
                    var unionId = "";
                    SessionBag? sessionBag = SessionContainer.UpdateSession(null, jsonResult.openid, jsonResult.session_key, unionId);

                    var res = await wxUserService.entityRepository.AsQueryable()
                        .Where(te => te.OpenId == jsonResult.openid)
                        .FirstAsync();

                    if (res == null)
                        await wxUserService.AddAsync(new WxUser { OpenId = jsonResult.openid, NickName = req.Name, AvatarUrl = req.Avatar, Phone = req.Phone });

                    //注意：生产环境下SessionKey属于敏感信息，不能进行传输！
                    return Ok(new JResult(new { openId = jsonResult.openid }));
                }
                else
                {
                    return Ok(new JResult(false, new { msg = jsonResult.errmsg }));
                }
            }
            catch (Exception ex)
            {
                return Ok(new JResult(false, new { msg = ex.Message }));
            }
        }

        [HttpGet("get")]
        public IActionResult Get(string openid)
        {
            var user = wxUserService.entityRepository.AsQueryable()
                .Where(te => te.OpenId == openid)
                .First();
            return Ok(user?.TotalScore ?? null);
        }



        [HttpGet("SetDeskInfo")]
        public IActionResult SetDeskInfo(int id, int desktopId, string openId)
        {
            try
            {
                var user = wxUserService.entityRepository.AsQueryable()
                    .Where(te => te.OpenId == openId)
                    .First();
                wxService.UpdateDeskInfo(id, desktopId, user);
                return Ok(true);

            }
            catch(Exception)
            {
                return BadRequest();
            }

        }

        [HttpGet("GetBattleInfo")]
        public IActionResult GetBattleInfo()
        {
            return Ok(new JResult(wxService.GetBattleMsg()));
        }


        [HttpGet("GetTotalList")]
        public async Task<IActionResult> GetTotalList()
        {
            List<WxUser>? Users = await wxUserService.entityRepository.AsQueryable().OrderByDescending(x => x.TotalScore).Take(50).ToListAsync();
            var muser = Users.Adapt<List<MWxUser>>();
            return Ok(new JResult(muser));
        }

        [HttpGet("GetTodayList")]
        public IActionResult GetTodayList()
        {
            return Ok(new JResult(wxService.GetTodayList()));
        }

        [HttpGet("Order")]
        public async Task<IActionResult> Order(AppointmentDto dto)
        {
            var result = await wxService.Order(dto);
            return Ok(new JResult(result));
        }

        [HttpGet("SignIn")]
        public async Task<IActionResult> SignIn(string openId)
        {
            var appoint = await appointUserService.entityRepository.AsQueryable()
                .Where(te => te.UserId == openId)
                .FirstAsync();
            if (appoint != null)
            {
                appoint.State = 1;
                await appointUserService.UpdateAsync(appoint);
                return Ok(new JResult(true));
            }
            else
            {
                return Ok(new JResult(false));
            }
        }


        [HttpGet("getPhone")]
        public async Task<IActionResult> GetPhone(string code)
        {
            var result = await BusinessApi.GetUserPhoneNumberAsync(WxOpenAppId, code);
            return Ok(new JResult(result));
        }

        [HttpGet("GetOrderList")]
        public async Task<IActionResult> GetOrderList()
        {
            //get date yyyyMMdd
            var date = int.Parse(DateTime.Now.ToString("yyyyMMdd") + "0");
            //and + 7 days
            var enddate = int.Parse(DateTime.Now.AddDays(7).ToString("yyyyMMdd") + "7");

            var list = await appointListService.entityRepository.AsQueryable()
                .Where(te => te.Id >= date && te.Id <= enddate)
                .Select(te => new { te.Id, te.Count })
                .ToListAsync();

            return Ok(new JResult(list));

        }
    }
}
