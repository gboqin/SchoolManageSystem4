using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolManageSystem.Common.Controllers;
using SchoolManageSystem.Dto.RequestParam;
using SchoolManageSystem.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolManageSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseUserController
    {
        private IUserService userService { get; set; }
        public UserController(IUserService _userService)
        {
            userService = _userService;
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="loginParam"></param>
        /// <returns></returns>
        //[HttpPost, Route("login"), AllowAnonymous]
        [HttpGet, Route("login"), AllowAnonymous]
        public async Task<ActionResult> Login()//Login(LoginParam loginParam)
        {
            var loginParam = new LoginParam
            {
                Email = "levywang123@gmail.com",
                Password = "admin",
                Uuid = "11",
                Captcha = "22"
            };
            var result = await userService.LoginAsync(loginParam);
            return MyJson(result);
        }

        [HttpGet, Route("login1"), AllowAnonymous]
        public async Task<ActionResult> Login1(LoginParam loginParam)
        {
            var result = await userService.LoginAsync(loginParam);
            return MyJson(result);
        }

        [HttpPost, Route("login2"), AllowAnonymous]
        public async Task<ActionResult> Login2(LoginParam loginParam)
        {
            var result = await userService.LoginAsync(loginParam);
            return MyJson(result);
        }
    }
}
