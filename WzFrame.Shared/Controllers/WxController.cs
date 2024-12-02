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

namespace WzFrame.Shared.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WxController : ControllerBase
    {
        private readonly WxService wxService;
        private readonly EntityService<WxUser> wxUserService;
        private const string WxOpenAppId = "wx3dfbaf4673340e6e";
        private const string WxOpenAppSecret = "533e87ac8c7c96e8ca94f9370fc8b6da";

        public WxController(WxService wxService, EntityService<WxUser> wxUserService)
        {
            this.wxService = wxService;
            this.wxUserService = wxUserService;
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


                    await wxUserService.AddAsync(new WxUser { OpenId = jsonResult.openid, NickName = req.Name, AvatarUrl = req.Avatar }); 

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
            return Ok(user.TotalScore);
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





    }
}
