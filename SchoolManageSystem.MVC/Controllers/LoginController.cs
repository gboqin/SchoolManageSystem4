using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SchoolManageSystem.Basics.Enums;
using SchoolManageSystem.Basics.Helpers;
using SchoolManageSystem.Basics.ResponseResults;
using SchoolManageSystem.Dto.CusEntity;
using SchoolManageSystem.Dto.RequestParam;
using SchoolManageSystem.Dto.SystemBo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SchoolManageSystem.MVC.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Index2()
        {
            return View();
        }

        public IActionResult Index3()
        {
            ViewBag.AA = "abc";
            TempData["BB"] = "BBB";
            return View();
        }

        public IActionResult Index4()
        {
            return View();
        }

        public IActionResult Index5()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> LoginNoVCode(LoginParam loginParam)
        {
            ResponseResult<UserDto> user;
            //string uid = "";
            //string token = "";
            var client = new HttpClient();
            //在传送复杂嵌套时，一定要把对象转化成json字符串
            //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //client.DefaultRequestHeaders.Add("Accept", "application/json");
            //client.DefaultRequestHeaders.Add("ContentType", "application/json");
            //client.DefaultRequestHeaders.Add("DataType", "application/json");
            ////StringContent strcontent = new StringContent(JsonConvert.SerializeObject(loginParam), Encoding.UTF8, "application/json");
            ////Reqams = new RequestParams();
            //client.BaseAddress = new System.Uri("http://localhost:36774");
            //var response = await client.GetAsync($"/api/user/login1?loginParam=" + JsonConvert.SerializeObject(loginParam));

            //var _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            client.BaseAddress = new System.Uri("http://localhost:49183");
            string content = JsonConvert.SerializeObject(loginParam);
            var buffer = Encoding.UTF8.GetBytes(content);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var response = await client.PostAsync($"/api/user/login2", byteContent).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                var content1 = await response.Content.ReadAsStringAsync();
                user = System.Text.Json.JsonSerializer.Deserialize<ResponseResult<UserDto>>(content1, _options);
                var lu = new LoginUserBo
                {
                    uid = response.Headers.GetValues("uid").FirstOrDefault(),
                    RoleList = user.Data.RoleList,
                    token = response.Headers.GetValues("token").FirstOrDefault(),
                    sessionId = response.Headers.GetValues("sessionId").FirstOrDefault()
                };

                //1.创建声明主题 指定认证方式 这里使用cookie
                var claimIdentity = new ClaimsIdentity("Cookie");
                //2.创建cookie 保存用户信息，使用claim。将序列化用户信息并将其存储在cookie中
                claimIdentity.AddClaim(new Claim(ClaimTypes.Sid, lu.uid));
                claimIdentity.AddClaim(new Claim(ClaimTypes.Authentication, lu.token));
                claimIdentity.AddClaim(new Claim(ClaimTypes.SerialNumber, lu.sessionId));
                //多个角色
                user.Data.RoleList.ForEach(role => claimIdentity.AddClaim(new Claim(ClaimTypes.Role, role)));
                //单个角色
                //claimIdentity.AddClaim(new Claim(ClaimTypes.Role, user.Data.RoleList));
                //3.配置认证属性 比如过期时间，是否持久化。。。。
                var claimsPrincipal = new ClaimsPrincipal(claimIdentity);
                //4.登录,SignInAsync 创建一个加密的 cookie ，并将其添加到当前响应中。
                //await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                //    new ClaimsPrincipal(claimIdentity),new AuthenticationProperties {
                //                                 IsPersistent=true, //cookie过期时间设置为持久
                //                                 ExpiresUtc= DateTime.UtcNow.AddSeconds(20)      //设置过期20秒
                //});
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

                return Json(new ResponseResult().Succeed(user));
            }
            else
            {
                //Console.WriteLine(response.StatusCode);
                //Console.WriteLine(await response.Content.ReadAsStringAsync());
                return Json(new ResponseResult().Fail(ResponseCode.LoginFail, "error", response));
            }

        }

        //[HttpPost]
        [HttpGet]
        public async Task<JsonResult> Login(LoginParam loginParam)
        {
            var code =  HttpContext.Session.GetString("ValidateCode");
            if (loginParam.Captcha != HttpContext.Session.GetString("ValidateCode"))
            {
                return Json(new ResponseResult().Fail(ResponseCode.VerifyCodeFail, "验证码不符，请输入正确的验证码！", loginParam));
            }
            ResponseResult<UserDto> user;
            //string uid = "";
            //string token = "";
            var client = new HttpClient();
            //在传送复杂嵌套时，一定要把对象转化成json字符串
            //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //client.DefaultRequestHeaders.Add("Accept", "application/json");
            //client.DefaultRequestHeaders.Add("ContentType", "application/json");
            //client.DefaultRequestHeaders.Add("DataType", "application/json");
            ////StringContent strcontent = new StringContent(JsonConvert.SerializeObject(loginParam), Encoding.UTF8, "application/json");
            ////Reqams = new RequestParams();
            //client.BaseAddress = new System.Uri("http://localhost:36774");
            //var response = await client.GetAsync($"/api/user/login1?loginParam=" + JsonConvert.SerializeObject(loginParam));

            //var _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            client.BaseAddress = new System.Uri("http://localhost:49183");
            string content = JsonConvert.SerializeObject(loginParam);
            var buffer = Encoding.UTF8.GetBytes(content);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var response = await client.PostAsync($"/api/user/login2", byteContent).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                var content1 = await response.Content.ReadAsStringAsync();
                user = System.Text.Json.JsonSerializer.Deserialize<ResponseResult<UserDto>>(content1, _options);
                var lu = new LoginUserBo { uid = response.Headers.GetValues("uid").FirstOrDefault(),
                    RoleList = user.Data.RoleList,
                    token = response.Headers.GetValues("token").FirstOrDefault(),
                    sessionId = response.Headers.GetValues("sessionId").FirstOrDefault()
                };
                
                //1.创建声明主题 指定认证方式 这里使用cookie
                var claimIdentity = new ClaimsIdentity("Cookie");
                //2.创建cookie 保存用户信息，使用claim。将序列化用户信息并将其存储在cookie中
                claimIdentity.AddClaim(new Claim(ClaimTypes.Sid, lu.uid));
                claimIdentity.AddClaim(new Claim(ClaimTypes.Authentication, lu.token));
                claimIdentity.AddClaim(new Claim(ClaimTypes.SerialNumber, lu.sessionId));
                //多个角色
                user.Data.RoleList.ForEach(role => claimIdentity.AddClaim(new Claim(ClaimTypes.Role, role)));
                //单个角色
                //claimIdentity.AddClaim(new Claim(ClaimTypes.Role, user.Data.RoleList));
                //3.配置认证属性 比如过期时间，是否持久化。。。。
                var claimsPrincipal = new ClaimsPrincipal(claimIdentity);
                //4.登录,SignInAsync 创建一个加密的 cookie ，并将其添加到当前响应中。
                //await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                //    new ClaimsPrincipal(claimIdentity),new AuthenticationProperties {
                //                                 IsPersistent=true, //cookie过期时间设置为持久
                //                                 ExpiresUtc= DateTime.UtcNow.AddSeconds(20)      //设置过期20秒
                //});
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

                return Json(new ResponseResult().Succeed(user));
            }
            else
            {
                //Console.WriteLine(response.StatusCode);
                //Console.WriteLine(await response.Content.ReadAsStringAsync());
                return Json(new ResponseResult().Fail(ResponseCode.LoginFail, "error", response));
            }
         
        }
        public ActionResult GetValidateCode()
        {
            ValidateCode vCode = new ValidateCode();
            string code = vCode.CreateValidateCode(4);
            HttpContext.Session.SetString("ValidateCode", code);
            //Session["ValidateCode"] = code;
            byte[] bytes = vCode.CreateValidateGraphic(code);
            return File(bytes, @"image/gif");
        }

        public IActionResult LogOut()
        {
            //退出很简单，主要用户清除cookie
            if (User.Identity.IsAuthenticated)
            {
                Task.Run(async () => { await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme); }).Wait();
            }

            //销毁 Claim 的 Cookie 后，可以跳转到登录页面
            return RedirectToAction("Index", "Login");
        }
    }
}
