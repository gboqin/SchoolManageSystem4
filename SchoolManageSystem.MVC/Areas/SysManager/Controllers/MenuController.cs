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
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManageSystem.MVC.Areas.SysManager.Controllers
{
    [Authorize(Roles = "System")]
    [Area("SysManager")]
    [Route("SysManager/[controller]/[action]")]
    public class MenuController : Controller
    {
        HttpClient client = new HttpClient();
        string uid, token, sessionId;
        string baseUrl = "http://localhost:49183";
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SetMenuInfo()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetMenus()
        {
            ResponseResult<List<MenuDto>> menus;

            if (HttpContext.User.Identity.IsAuthenticated)
            {
                uid = User.FindFirst(ClaimTypes.Sid).Value;
                token = User.FindFirst(ClaimTypes.Authentication).Value;
                sessionId = User.FindFirst(ClaimTypes.SerialNumber).Value;

                client.DefaultRequestHeaders.Add("uid", uid);
                client.DefaultRequestHeaders.Add("token", token);
                client.DefaultRequestHeaders.Add("sessionId", sessionId);
                var response = await client.GetAsync(baseUrl + "/api/Menu/AllMenus");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    menus = JsonConvert.DeserializeObject<ResponseResult<List<MenuDto>>>(content);
                    JsonSerializerSettings settings = new JsonSerializerSettings();
                    //string json = JsonConvert.SerializeObject(menus, settings);
                    //return Json(new ResponseResult().Succeed(json));
                    //Index7
                    return Json(new { code = 0, msg = "", count = menus.Data.Count, data = menus.Data });

                    //return Json(new
                    //{
                    //    data = menus.Data
                    //});
                }
                return Json(new ResponseResult().Fail(ResponseCode.Fail, "error", response));
            }
            return Json(new ResponseResult().Fail(ResponseCode.Fail, "未登录！", "error"));
        }

        [HttpGet]
        public async Task<JsonResult> GetSelectMenus()
        {
            ResponseResult<List<SelectTreeDto>> menus;

            if (HttpContext.User.Identity.IsAuthenticated)
            {
                uid = User.FindFirst(ClaimTypes.Sid).Value;
                token = User.FindFirst(ClaimTypes.Authentication).Value;
                sessionId = User.FindFirst(ClaimTypes.SerialNumber).Value;

                client.DefaultRequestHeaders.Add("uid", uid);
                client.DefaultRequestHeaders.Add("token", token);
                client.DefaultRequestHeaders.Add("sessionId", sessionId);
                var response = await client.GetAsync(baseUrl + "/api/Menu/TreeSelectMenus");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    menus = JsonConvert.DeserializeObject<ResponseResult<List<SelectTreeDto>>>(content);
                    //JsonSerializerSettings settings = new JsonSerializerSettings();
                    //string json = JsonConvert.SerializeObject(menus, settings);
                    return Json(menus.Data);
                }
                return Json(new ResponseResult().Fail(ResponseCode.Fail, "error", response));
            }
            return Json(new ResponseResult().Fail(ResponseCode.Fail, "未登录！", "error"));
        }

        [HttpGet]
        public async Task<JsonResult> GetTestMenus()
        {
            ResponseResult<List<TestMenuDto>> menus;

            if (HttpContext.User.Identity.IsAuthenticated)
            {
                uid = User.FindFirst(ClaimTypes.Sid).Value;
                token = User.FindFirst(ClaimTypes.Authentication).Value;
                sessionId = User.FindFirst(ClaimTypes.SerialNumber).Value;

                client.DefaultRequestHeaders.Add("uid", uid);
                client.DefaultRequestHeaders.Add("token", token);
                client.DefaultRequestHeaders.Add("sessionId", sessionId);
                var response = await client.GetAsync(baseUrl + "/api/Menu/AllTestMenus");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    menus = JsonConvert.DeserializeObject<ResponseResult<List<TestMenuDto>>>(content);
                    JsonSerializerSettings settings = new JsonSerializerSettings();
                    //string json = JsonConvert.SerializeObject(menus, settings);
                    //return Json(new ResponseResult().Succeed(json));
                    //return Json(new{ code = 0 , msg = "" , count = menus.Data.Count, data = menus.Data });

                    return Json(new
                    {
                        status = new { code = 200, message = "操作成功" },
                        data = menus.Data
                    });
                }
                return Json(new ResponseResult().Fail(ResponseCode.Fail, "error", response));
            }
            return Json(new ResponseResult().Fail(ResponseCode.Fail, "未登录！", "error"));
        }

        [HttpPost]
        public async Task<JsonResult> AddMenu(TestMenuDto menuParam)
        {
            ResponseResult<TestMenuDto> menu;
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                uid = User.FindFirst(ClaimTypes.Sid).Value;
                token = User.FindFirst(ClaimTypes.Authentication).Value;
                sessionId = User.FindFirst(ClaimTypes.SerialNumber).Value;

                client.DefaultRequestHeaders.Add("uid", uid);
                client.DefaultRequestHeaders.Add("token", token);
                client.DefaultRequestHeaders.Add("sessionId", sessionId);

                var requestUri = string.Format("/api/Menu/AddMenu");
                string param = JsonConvert.SerializeObject(menuParam);
                var buffer = Encoding.UTF8.GetBytes(param);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                //var _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var response = await client.PostAsync(baseUrl + requestUri, byteContent).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    menu = JsonConvert.DeserializeObject<ResponseResult<TestMenuDto>>(content);
                    JsonSerializerSettings settings = new JsonSerializerSettings();
                    settings.MaxDepth = 10; //设置序列化的最大层数
                    string json = JsonConvert.SerializeObject(menu, settings);

                    return Json(new ResponseResult().Succeed(json));
                }
                return Json(new ResponseResult().Fail(ResponseCode.Fail, "error", response));
            }
            return Json(new ResponseResult().Fail(ResponseCode.Fail, "未登录！", "error"));
        }

        [HttpPost]
        public async Task<JsonResult> EdtMenu(TestMenuDto menuParam)
        {
            ResponseResult<TestMenuDto> menu;
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                uid = User.FindFirst(ClaimTypes.Sid).Value;
                token = User.FindFirst(ClaimTypes.Authentication).Value;
                sessionId = User.FindFirst(ClaimTypes.SerialNumber).Value;

                client.DefaultRequestHeaders.Add("uid", uid);
                client.DefaultRequestHeaders.Add("token", token);
                client.DefaultRequestHeaders.Add("sessionId", sessionId);

                var requestUri = string.Format("/api/Menu/EdtMenu");
                string param = JsonConvert.SerializeObject(menuParam);
                var buffer = Encoding.UTF8.GetBytes(param);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                //var _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var response = await client.PostAsync(baseUrl + requestUri, byteContent).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    menu = JsonConvert.DeserializeObject<ResponseResult<TestMenuDto>>(content);
                    JsonSerializerSettings settings = new JsonSerializerSettings();
                    settings.MaxDepth = 10; //设置序列化的最大层数
                    string json = JsonConvert.SerializeObject(menu, settings);

                    return Json(new ResponseResult().Succeed(json));
                }
                return Json(new ResponseResult().Fail(ResponseCode.Fail, "error", response));
            }
            return Json(new ResponseResult().Fail(ResponseCode.Fail, "未登录！", "error"));
        }

        [HttpDelete]
        public async Task<JsonResult> DelMenu(TestMenuDto menuParam)
        {
            ResponseResult<int> changedCount;
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                uid = User.FindFirst(ClaimTypes.Sid).Value;
                token = User.FindFirst(ClaimTypes.Authentication).Value;
                sessionId = User.FindFirst(ClaimTypes.SerialNumber).Value;

                client.DefaultRequestHeaders.Add("uid", uid);
                client.DefaultRequestHeaders.Add("token", token);
                client.DefaultRequestHeaders.Add("sessionId", sessionId);

                var requestUri = string.Format("/api/Menu/DelMenu");
                string param = JsonConvert.SerializeObject(menuParam);
                var buffer = Encoding.UTF8.GetBytes(param);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                //var _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var response = await client.PostAsync(baseUrl + requestUri, byteContent).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    changedCount = JsonConvert.DeserializeObject<ResponseResult<int>>(content);
                    JsonSerializerSettings settings = new JsonSerializerSettings();
                    settings.MaxDepth = 10; //设置序列化的最大层数
                    string json = JsonConvert.SerializeObject(changedCount, settings);

                    return Json(new ResponseResult().Succeed(json));
                }
                return Json(new ResponseResult().Fail(ResponseCode.Fail, "error", response));
            }
            return Json(new ResponseResult().Fail(ResponseCode.Fail, "未登录！", "error"));
        }
    }
}
