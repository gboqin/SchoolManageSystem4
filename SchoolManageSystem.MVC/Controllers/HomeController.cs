using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SchoolManageSystem.Dto.CusEntity;
using SchoolManageSystem.MVC.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Net;
using System.Text.Json;
using SchoolManageSystem.Basics.ResponseResults;
using SchoolManageSystem.Dto.RequestParam;
using Newtonsoft.Json;
using System.Text;
using System.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace SchoolManageSystem.MVC.Controllers
{
    //基本授权
    //[Authorize]
    //策略授权--自定义授权
    //[Authorize(Policy = "CustomPolicy")
    //[Authorize(Policy = "AdminPolicy")]
    [Authorize(Roles = "System,Admin")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            //获取认证信息
            //如果HttpContext.User.Identity.IsAuthenticated为true，
            //或者HttpContext.User.Claims.Count()大于0表示用户已经登录
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                //这里通过 HttpContext.User.Claims 可以将我们在Login这个Action中存储到cookie中的所有
                //claims键值对都读出来，比如我们刚才定义的UserName的值Wangdacui就在这里读取出来了
                //var sid = HttpContext.User.Claims.First().Value;
                var sid2 = User.FindFirst(ClaimTypes.Sid).Value;
                var token = User.FindFirst(ClaimTypes.Authentication).Value;
            }
            return View();
        }
      
        public async Task<IActionResult> Privacy()
        {
            /*using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri("www.cnblogs.com");
                var userId = 1;
                var response = await client.GetAsync($"/api/messages/user-{userId}/unread/count");
                if (response.IsSuccessStatusCode)
                {
                    var unreadCount = response.Headers.GetValues("X-RESULT-COUNT").FirstOrDefault();
                    Console.WriteLine(unreadCount);
                    Assert.Equal(10, int.Parse(unreadCount));
                }
                else
                {
                    Console.WriteLine(response.StatusCode);
                    Console.WriteLine(await response.Content.ReadAsStringAsync());
                }
            }*/
            await GetUser();
            return View();
        }

        //public async Task<UserDto> GetUser()
        public async Task  GetUser()
        {
            var loginParam = new LoginParam
            {
                Email = "levywang123@gmail.com",
                Password = "admin",
                Uuid = "11",
                Captcha = "22"
            };

            ResponseResult<UserDto> user;
            string uid="";
            string token="";
            var client = new HttpClient();
            client.BaseAddress = new System.Uri("http://localhost:36774");

            //Post(带参数),在传送复杂嵌套时，一定要把对象转化成json字符串
            
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
                token = response.Headers.GetValues("token").FirstOrDefault();
                uid = response.Headers.GetValues("uid").FirstOrDefault();
                Console.WriteLine(token);
            }
            else
            {
                Console.WriteLine(response.StatusCode);
                Console.WriteLine(await response.Content.ReadAsStringAsync());
            }
            
            //Get无参数
            /* var response = await client.GetAsync($"/api/user/login");
            var _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                user = JsonSerializer.Deserialize<ResponseResult<UserDto>>(content, _options);
                token = response.Headers.GetValues("token").FirstOrDefault();
                uid = response.Headers.GetValues("uid").FirstOrDefault();
                Console.WriteLine(token);
            }
            else
            {
                Console.WriteLine(response.StatusCode);
                Console.WriteLine(await response.Content.ReadAsStringAsync());
            }*/

            var client2 = new HttpClient();
            client2.BaseAddress = new System.Uri("http://localhost:49183");
            client2.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            client2.DefaultRequestHeaders.Add("uid", uid);
            var response2 = await client2.GetAsync($"/WeatherForecast");
            var _options2 = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            if (response2.IsSuccessStatusCode)
            {
                var content2 = await response2.Content.ReadAsStringAsync();
                var user2 = System.Text.Json.JsonSerializer.Deserialize<List<WeatherForecast>>(content2, _options2);
            }
            else
            {
                Console.WriteLine(response2.StatusCode);
                Console.WriteLine(await response2.Content.ReadAsStringAsync());
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
