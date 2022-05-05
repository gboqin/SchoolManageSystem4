using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SchoolManageSystem.Basics.Enums;
using SchoolManageSystem.Basics.ResponseResults;
using SchoolManageSystem.Dto.CusEntity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SchoolManageSystem.MVC.Areas.SysManager.Controllers
{
    [Authorize(Roles = "System")]
    [Area("SysManager")]
    [Route("SysManager/[controller]/[action]")]
    public class SysManagerController : Controller
    {
        public IActionResult Index()
        {
            //ResponseResult<List<MenuDto>> menus;
            //var client = new HttpClient();

            //获取认证信息
            //如果HttpContext.User.Identity.IsAuthenticated为true，
            //或者HttpContext.User.Claims.Count()大于0表示用户已经登录
            //if (HttpContext.User.Identity.IsAuthenticated)
            /*if (HttpContext.User.Identity.IsAuthenticated)
            {
                //这里通过 HttpContext.User.Claims 可以将我们在Login这个Action中存储到cookie中的所有
                //claims键值对都读出来，比如我们刚才定义的UserName的值Wangdacui就在这里读取出来了
                var uid = User.FindFirst(ClaimTypes.Sid).Value;
                var token = User.FindFirst(ClaimTypes.Authentication).Value;
                var sessionId = User.FindFirst(ClaimTypes.SerialNumber).Value;

                client.DefaultRequestHeaders.Add("uid", uid);
                client.DefaultRequestHeaders.Add("token", token);
                client.DefaultRequestHeaders.Add("sessionId", sessionId);
                client.BaseAddress = new System.Uri("http://localhost:36774");

                var response = await client.GetAsync($"/api/Menu/NavTreeMenus");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    menus = JsonConvert.DeserializeObject<ResponseResult<List<MenuDto>>>(content);
                }
            }*/
            return View();
        }
        public IActionResult Index2()
        {
            return View();
        }

        public IActionResult Index3()
        {
            return View();
        }

        public async Task<JsonResult> GetLeftMenu()
        {
            ResponseResult<List<MenuDto>> menus;
            var client = new HttpClient();

            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var uid = User.FindFirst(ClaimTypes.Sid).Value;
                var token = User.FindFirst(ClaimTypes.Authentication).Value;
                var sessionId = User.FindFirst(ClaimTypes.SerialNumber).Value;

                client.DefaultRequestHeaders.Add("uid", uid);
                client.DefaultRequestHeaders.Add("token", token);
                client.DefaultRequestHeaders.Add("sessionId", sessionId);
                client.BaseAddress = new System.Uri("http://localhost:49183");

                var response = await client.GetAsync($"/api/Menu/NavTreeMenus");
                //var _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    //menus = JsonSerializer.Deserialize<ResponseResult<List<MenuDto>>>(content, _options);
                    menus = JsonConvert.DeserializeObject<ResponseResult<List<MenuDto>>>(content);

                    //https://blog.csdn.net/u011127019/article/details/72801033/
                    //对于Newtonsoft.Json默认 已经处理过循环引用,也就是对于关联表的 对象或列表都不会序列化出来。
                    //设置循环引用，及引用类型序列化的层数。
                    //注：目前在 EF core中目前不支持延迟加载，无所谓循环引用了
                    JsonSerializerSettings settings = new JsonSerializerSettings();
                    //去除空值
                    //settings.NullValueHandling = NullValueHandling.Ignore;
                    //settings.Formatting = Formatting.Indented;
                    settings.MaxDepth = 10; //设置序列化的最大层数
                    //settings.StringEscapeHandling = StringEscapeHandling.EscapeHtml;
          
                    //指定如何处理循环引用，None--不序列化，Error-抛出异常，Serialize--仍要序列化
                    //settings.ReferenceLoopHandling = ReferenceLoopHandling.Serialize;
                    //string json = JsonConvert.SerializeObject(menus, settings);
                    string json = JsonConvert.SerializeObject(menus, settings);

                    return Json(new ResponseResult().Succeed(json));
                }
                return Json(new ResponseResult().Fail(ResponseCode.Fail, "error", response));
            }
            return Json(new ResponseResult().Fail(ResponseCode.Fail, "未登录！", "error"));
        }
    }
}
