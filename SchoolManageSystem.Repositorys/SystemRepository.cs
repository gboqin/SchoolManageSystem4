using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using SchoolManageSystem.Basics.Config;
using SchoolManageSystem.Basics.Helpers;
using SchoolManageSystem.Basics.IDCode.Snowflake;
using SchoolManageSystem.Basics.ResponseResults;
using SchoolManageSystem.Common.Cache.Redis;
using SchoolManageSystem.Common.NLog;
using SchoolManageSystem.Dto.RequestParam;
using SchoolManageSystem.IRepositorys;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManageSystem.Repositorys
{
    public class SystemRepository : ISystemRepository
    {
        public IConnection RabbitConnection { get; set; }

        /// <summary>
        /// 验证码5分钟过期
        /// </summary>
        private static readonly TimeSpan CaptchaExpired = TimeSpan.FromMinutes(5);

        public RedisClient RedisClient { get; set; }

        public IConfiguration IConfiguration { get; set; }
        public IdWorker IdWorker { get; set; }
        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <returns></returns>
        public async Task<CaptchaResult> GetCaptchaResultAsync()
        {
            CaptchaResult captcha;
            try
            {
                captcha = CaptchaHelper.CreateVerifyCodeImage(CaptchaHelper.GenerateCaptchaCode());
            }
            catch (Exception e)
            {
                LogHelper.Logger.Fatal(e, "验证码生成 失败");
                return null;
            }
            //var captcha = CaptchaHelper.CreateImage(CaptchaHelper.GenerateCaptchaCode());
            var uuid = IdWorker.NextId();
            var key = CacheKeys.CAPTCHA + uuid;
            await RedisClient.SetAsync(key, captcha.Captcha, CaptchaExpired);
            captcha.Uuid = uuid.ToString();
            return captcha;
        }

        /// <summary>
        /// 验证登录时的验证码
        /// </summary>
        /// <param name="loginParam"></param>
        /// <returns></returns>
        public async Task<string> VerificationLoginAsync(LoginParam loginParam)
        {
            var key = CacheKeys.CAPTCHA + loginParam.Uuid;
            var captcha = await RedisClient.GetAsync<string>(key);
            if (captcha == null)
            {
                return "验证码已失效";
            }

            await RedisClient.DeleteAsync(key);
            if (!string.Equals(loginParam.Captcha, captcha, StringComparison.OrdinalIgnoreCase))
            {
                return "验证码错误";
            }
            return "验证通过";
        }
    }
}
