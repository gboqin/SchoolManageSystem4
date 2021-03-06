using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SchoolManageSystem.Basics.Enums;
using SchoolManageSystem.Basics.ResponseResults;
using SchoolManageSystem.Dto.CusEntity;
using SchoolManageSystem.Dto.RequestParam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
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

        [HttpGet]
        public async Task<JsonResult> GetMenus()
        {
            ResponseResult<List<Dictionary<string, object>>> menus;

            if (HttpContext.User.Identity.IsAuthenticated)
            {
                uid = User.FindFirst(ClaimTypes.Sid).Value;
                token = User.FindFirst(ClaimTypes.Authentication).Value;
                sessionId = User.FindFirst(ClaimTypes.SerialNumber).Value;

                client.DefaultRequestHeaders.Add("uid", uid);
                client.DefaultRequestHeaders.Add("token", token);
                client.DefaultRequestHeaders.Add("sessionId", sessionId);
                var response = await client.GetAsync(baseUrl + "/api/Role/TreeMenus");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    menus = JsonConvert.DeserializeObject<ResponseResult<List<Dictionary<string,object>>>>(content);
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

        public async Task<JsonResult> GetMenuIdsByRId(long Id)
        {
            ResponseResult<List<long>> Ids;

            if (HttpContext.User.Identity.IsAuthenticated)
            {
                uid = User.FindFirst(ClaimTypes.Sid).Value;
                token = User.FindFirst(ClaimTypes.Authentication).Value;
                sessionId = User.FindFirst(ClaimTypes.SerialNumber).Value;

                client.DefaultRequestHeaders.Add("uid", uid);
                client.DefaultRequestHeaders.Add("token", token);
                client.DefaultRequestHeaders.Add("sessionId", sessionId);

                var requestUri = string.Format("/api/Role/FirstMenuIdsByRoleId?roleId={0}", Id);
                var response = await client.PostAsync(baseUrl + requestUri, null); ;
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Ids = JsonConvert.DeserializeObject<ResponseResult<List<long>>>(content);
                    //https://blog.csdn.net/u011127019/article/details/72801033/
                    //对于Newtonsoft.Json默认 已经处理过循环引用,也就是对于关联表的 对象或列表都不会序列化出来。
                    //设置循环引用，及引用类型序列化的层数。
                    //注：目前在 EF core中目前不支持延迟加载，无所谓循环引用了
                    JsonSerializerSettings settings = new JsonSerializerSettings();
                    settings.MaxDepth = 10; //设置序列化的最大层数
                    //指定如何处理循环引用，None--不序列化，Error-抛出异常，Serialize--仍要序列化
                    string json = JsonConvert.SerializeObject(Ids, settings);

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

        [HttpPost]
        public async Task<JsonResult> AddRole(RoleDto roleParam)
        {
            ResponseResult<RoleDto> role;
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                uid = User.FindFirst(ClaimTypes.Sid).Value;
                token = User.FindFirst(ClaimTypes.Authentication).Value;
                sessionId = User.FindFirst(ClaimTypes.SerialNumber).Value;

                client.DefaultRequestHeaders.Add("uid", uid);
                client.DefaultRequestHeaders.Add("token", token);
                client.DefaultRequestHeaders.Add("sessionId", sessionId);

                var requestUri = string.Format("/api/Role/AddRole");
                string param = JsonConvert.SerializeObject(roleParam);
                var buffer = Encoding.UTF8.GetBytes(param);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                //var _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var response = await client.PostAsync(baseUrl + requestUri, byteContent).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    role = JsonConvert.DeserializeObject<ResponseResult<RoleDto>>(content);
                    JsonSerializerSettings settings = new JsonSerializerSettings();
                    settings.MaxDepth = 10; //设置序列化的最大层数
                    string json = JsonConvert.SerializeObject(role, settings);

                    return Json(new ResponseResult().Succeed(json));
                }
                return Json(new ResponseResult().Fail(ResponseCode.Fail, "error", response));
            }
            return Json(new ResponseResult().Fail(ResponseCode.Fail, "未登录！", "error"));
        }

        [HttpPost]
        public async Task<JsonResult> EdtRole(RoleDto roleParam)
        {
            ResponseResult<RoleDto> role;
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                uid = User.FindFirst(ClaimTypes.Sid).Value;
                token = User.FindFirst(ClaimTypes.Authentication).Value;
                sessionId = User.FindFirst(ClaimTypes.SerialNumber).Value;

                client.DefaultRequestHeaders.Add("uid", uid);
                client.DefaultRequestHeaders.Add("token", token);
                client.DefaultRequestHeaders.Add("sessionId", sessionId);

                var requestUri = string.Format("/api/Role/EditRole");
                string param = JsonConvert.SerializeObject(roleParam);
                var buffer = Encoding.UTF8.GetBytes(param);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                //var _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var response = await client.PostAsync(baseUrl + requestUri, byteContent).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    role = JsonConvert.DeserializeObject<ResponseResult<RoleDto>>(content);
                    JsonSerializerSettings settings = new JsonSerializerSettings();
                    settings.MaxDepth = 10; //设置序列化的最大层数
                    string json = JsonConvert.SerializeObject(role, settings);

                    return Json(new ResponseResult().Succeed(json));
                }
                return Json(new ResponseResult().Fail(ResponseCode.Fail, "error", response));
            }
            return Json(new ResponseResult().Fail(ResponseCode.Fail, "未登录！", "error"));
        }

        [HttpDelete]
        public async Task<JsonResult> DelRole(RoleDto roleParam)
        {
            ResponseResult<RoleDto> role;
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                uid = User.FindFirst(ClaimTypes.Sid).Value;
                token = User.FindFirst(ClaimTypes.Authentication).Value;
                sessionId = User.FindFirst(ClaimTypes.SerialNumber).Value;

                client.DefaultRequestHeaders.Add("uid", uid);
                client.DefaultRequestHeaders.Add("token", token);
                client.DefaultRequestHeaders.Add("sessionId", sessionId);

                var requestUri = string.Format("/api/Role/DelRole");
                string param = JsonConvert.SerializeObject(roleParam);
                var buffer = Encoding.UTF8.GetBytes(param);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                //var _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var response = await client.PostAsync(baseUrl + requestUri, byteContent).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    role = JsonConvert.DeserializeObject<ResponseResult<RoleDto>>(content);
                    JsonSerializerSettings settings = new JsonSerializerSettings();
                    settings.MaxDepth = 10; //设置序列化的最大层数
                    string json = JsonConvert.SerializeObject(role, settings);

                    return Json(new ResponseResult().Succeed(json));
                }
                return Json(new ResponseResult().Fail(ResponseCode.Fail, "error", response));
            }
            return Json(new ResponseResult().Fail(ResponseCode.Fail, "未登录！", "error"));
        }

        public IActionResult AssignPermissions()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> Assign(AssingParam assingParam)
        {
            ResponseResult<int> result;
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                uid = User.FindFirst(ClaimTypes.Sid).Value;
                token = User.FindFirst(ClaimTypes.Authentication).Value;
                sessionId = User.FindFirst(ClaimTypes.SerialNumber).Value;

                client.DefaultRequestHeaders.Add("uid", uid);
                client.DefaultRequestHeaders.Add("token", token);
                client.DefaultRequestHeaders.Add("sessionId", sessionId);

                var requestUri = string.Format("/api/Role/AssignPermissions");
                string param = JsonConvert.SerializeObject(assingParam);
                var buffer = Encoding.UTF8.GetBytes(param);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                //var _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var response = await client.PostAsync(baseUrl + requestUri, byteContent).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<ResponseResult<int>>(content);
                    JsonSerializerSettings settings = new JsonSerializerSettings();
                    settings.MaxDepth = 10; //设置序列化的最大层数
                    string json = JsonConvert.SerializeObject(result, settings);

                    return Json(new ResponseResult().Succeed(json));
                }
                return Json(new ResponseResult().Fail(ResponseCode.Fail, "error", response));
            }
            return Json(new ResponseResult().Fail(ResponseCode.Fail, "未登录！", "error"));
        }


    }
}
