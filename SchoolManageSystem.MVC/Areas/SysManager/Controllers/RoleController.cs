using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SchoolManageSystem.Basics.Enums;
using SchoolManageSystem.Basics.ResponseResults;
using SchoolManageSystem.Dto.CusEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SchoolManageSystem.MVC.Areas.SysManager.Controllers
{
    [Authorize(Roles = "System")]
    [Area("SysManager")]
    [Route("SysManager/[controller]/[action]")]
    public class RoleController : Controller
    {
        HttpClient client = new HttpClient();
        string uid,token,sessionId;
        string baseUrl = "http://localhost:49183";
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Index1()
        {
            return View();
        }

        public IActionResult Index2()
        {
            return View();
        }

        public async Task<JsonResult> GetRolesByName(string name)
        {

            ResponseResult<List<RoleDto>> roles;
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                uid = User.FindFirst(ClaimTypes.Sid).Value;
                token = User.FindFirst(ClaimTypes.Authentication).Value;
                sessionId = User.FindFirst(ClaimTypes.SerialNumber).Value;

                client.DefaultRequestHeaders.Add("uid", uid);
                client.DefaultRequestHeaders.Add("token", token);
                client.DefaultRequestHeaders.Add("sessionId", sessionId);

                var requestUri = string.Format("/api/Role/RolesByName?roleName={0}", name);
                var response = await client.PostAsync(baseUrl + requestUri, null);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    roles = JsonConvert.DeserializeObject<ResponseResult<List<RoleDto>>>(content);
                    //https://blog.csdn.net/u011127019/article/details/72801033/
                    //对于Newtonsoft.Json默认 已经处理过循环引用,也就是对于关联表的 对象或列表都不会序列化出来。
                    //设置循环引用，及引用类型序列化的层数。
                    //注：目前在 EF core中目前不支持延迟加载，无所谓循环引用了
                    JsonSerializerSettings settings = new JsonSerializerSettings();
                    settings.MaxDepth = 10; //设置序列化的最大层数
                    //指定如何处理循环引用，None--不序列化，Error-抛出异常，Serialize--仍要序列化
                    string json = JsonConvert.SerializeObject(roles, settings);

                    return Json(new ResponseResult().Succeed(json));
                }
                return Json(new ResponseResult().Fail(ResponseCode.Fail, "error", response));
            }
            return Json(new ResponseResult().Fail(ResponseCode.Fail, "未登录！", "error"));
        }

        public async Task<JsonResult> GetMenus()
        {
            ResponseResult<List<MenuDto>> menus;

            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var response = await client.GetAsync($"/api/Role/TreeMenus");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    menus = JsonConvert.DeserializeObject<ResponseResult<List<MenuDto>>>(content);
                    //https://blog.csdn.net/u011127019/article/details/72801033/
                    //对于Newtonsoft.Json默认 已经处理过循环引用,也就是对于关联表的 对象或列表都不会序列化出来。
                    //设置循环引用，及引用类型序列化的层数。
                    //注：目前在 EF core中目前不支持延迟加载，无所谓循环引用了
                    JsonSerializerSettings settings = new JsonSerializerSettings();
                    settings.MaxDepth = 10; //设置序列化的最大层数
                    //指定如何处理循环引用，None--不序列化，Error-抛出异常，Serialize--仍要序列化
                    string json = JsonConvert.SerializeObject(menus, settings);

                    return Json(new ResponseResult().Succeed(json));
                }
                return Json(new ResponseResult().Fail(ResponseCode.Fail, "error", response));
            }
            return Json(new ResponseResult().Fail(ResponseCode.Fail, "未登录！", "error"));
        }

        public IActionResult SetRoleInfo()
        {
            return View();
        }

    }
}
