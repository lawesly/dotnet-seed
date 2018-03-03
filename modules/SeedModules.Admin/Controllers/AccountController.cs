using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Seed.Mvc.Models;
using SeedModules.Admin.Models;
using SeedModules.Admin.Users;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Seed.Mvc.Extensions;
using Seed.Mvc.Filters;

namespace SeedModules.Admin.Controllers
{
    [Route("api/account")]
    [Authorize]
    public class AccountController : Controller
    {
        readonly SignInManager<IUser> _signInManager;

        public AccountController(SignInManager<IUser> signInManager)
        {
            _signInManager = signInManager;
        }

        [HttpPost("login"), HandleResult]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<LoginResult> Login([FromBody]LoginModel model, [FromQuery]string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.IsRemember, false);

                if (result.Succeeded)
                {
                    return new LoginResult(true)
                    {
                        ReturnUrl = returnUrl
                    };
                }
                else
                {
                    this.Throw("登录不成功");
                }
            }
            else
            {
                this.Throw("输入信息有误");
            }

            return new LoginResult();
        }

        [HttpPost("logout")]
        [ValidateAntiForgeryToken]
        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }
    }
}