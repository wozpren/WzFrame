using Lazy.Captcha.Core;
using Mapster;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WzFrame.Entity.DTO;
using WzFrame.Entity.Users;
using WzFrame.Shared.ApiResult;
using WzFrame.Shared.Services;
using static FastExpressionCompiler.ExpressionCompiler;

namespace WzFrame.Shared.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly EntityService<ApplicationUser> _entityService;
        private readonly ICaptcha captcha;
        public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, ICaptcha captcha, EntityService<ApplicationUser> entityService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            this.captcha = captcha;
            _entityService = entityService;
        }

        [HttpGet]
        public async Task<ActionResult<TResult<List<UserVO>>>> GetAllUser()
        {
            var res = await _entityService.GetAll();

            return Ok(new TResult<List<UserVO>>(res.Adapt<List<UserVO>>()));
        }



        [HttpPost("register")]
        public async Task<ActionResult<BaseResult>> Register(RegisterInputApi input)
        {
            if (!captcha.Validate(input.CaptchaId, input.Captcha))
            {
                return Ok(new BaseResult("验证码错误"));
            }
            var user = new ApplicationUser
            {
                UserName = input.UserName,
                PhoneNumber = input.UserName,
                EmailConfirmed = true
            };
            IdentityResult? result = await _userManager.CreateAsync(user, input.Password);
            if (result.Succeeded)
            {
                return Ok(new BaseResult());
            }
            else
            {
                return Ok(new BaseResult(result.Errors.Select(e => e.Description).ToArray()));
            }
        }



        [HttpPost("login")]
        public async Task<ActionResult<BaseResult>> Login(LoginInputApi input)
        {
            if (!captcha.Validate(input.CaptchaId, input.Captcha))
            {
                return Ok(new BaseResult("验证码错误"));
            }
            var result = await _signInManager.PasswordSignInAsync(input.UserName, input.Password, input.RememberMe, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                return Ok(new BaseResult());
            }
            else
            {
                return Ok(new BaseResult("用户名或密码错误"));
            }
        }

    }
}
