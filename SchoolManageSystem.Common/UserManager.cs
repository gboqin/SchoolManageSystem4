using SchoolManageSystem.Basics.Config;
using SchoolManageSystem.Basics.Enums;
using SchoolManageSystem.Basics.Exceptions;
using SchoolManageSystem.Basics.Extensions;
using SchoolManageSystem.Common.Cache;
using SchoolManageSystem.Common.JWT;
using SchoolManageSystem.Common.NLog;
using SchoolManageSystem.Dto.SystemBo;
using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolManageSystem.Common
{
    /// <summary>
    /// 用户处理类
    /// </summary>
    public class UserManager
    {
        public static UserCacheBo CurrentUser
        {
            get => GetCurrentUser(true);
            set
            {
                //var http = GlobalSystemWeb.HttpContext;
                //if (http != null)
                //{
                //    http.Items["user"] = value;
                //}
                UserCache.CreateUserCache(value);
            }
        }

        /// <summary>
        /// 获取当前用户
        /// </summary>
        /// <param name="throwException"></param>
        /// <returns></returns>
        private static UserCacheBo GetCurrentUser(bool throwException = false)
        {
            try
            {
                var http = GlobalSystemWeb.HttpContext;
                var cache = UserCache.Get(); //先从内存获取，再从redis获取
                if (cache != null)
                {
                    //http.Items["user"] = cache;
#if DEBUG
                    LogHelper.Logger.Debug("[LoginName]:{0}\n \n [UserId]:{1}\n [UserToken]:{2}", cache.NickName, cache.Id, cache.UserToken);
#endif
                    return cache;
                }
            }
            catch (Exception)
            {
                if (throwException)
                    throw new CustomSystemException("未授权，请重新登录", ResponseCode.Unauthorized);
            }

            return null;
        }

        /// <summary>
        /// token 验证正确性 单点登录
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        internal static SysConstants.TokenValidType ValidSessionId(string uid, string sessionId)
        {
            var cache = UserCache.Get(uid); //先从内存获取，再从redis获取
            if (cache == null)
            {
                return SysConstants.TokenValidType.LogonInvalid;
            }
            if (cache.SessionId == sessionId)
            {
#if DEBUG
                LogHelper.Logger.Debug("[LoginName]:{0}\n \n [UserId]:{1}\n [UserToken]:{2}", cache.NickName, cache.Id, cache.UserToken);
#endif
                return SysConstants.TokenValidType.Success;
            }
            return SysConstants.TokenValidType.LoggedInOtherPlaces;
        }

        /// <summary>
        /// 验证token
        /// </summary>
        /// <returns></returns>
        public static SysConstants.TokenValidType TokenLogin()//Authorization  Postman-Token
        {
            var token = GlobalSystemWeb.HttpContext.Request.Headers["token"];//访问请求要添加token头部文件变量
            //var token1 = GlobalSystemWeb.HttpContext.Request.Headers["Authorization"];//Authorization中添加Bearer Token
            if (string.IsNullOrEmpty(token))
            {
                return SysConstants.TokenValidType.LogonInvalid;
            }

            // 验证登录
            var str = TokenManager.ValidateToken(token, out DateTime date);
            if (!string.IsNullOrEmpty(str) || date > DateTime.Now)
            {
                var userDic = str.ToObject<Dictionary<string, string>>();
                GlobalSystemWeb.HttpContext.Items["uid"] = userDic["uid"];//访问请求要添加uid头部文件变量
                GlobalSystemWeb.HttpContext.Items["sessionId"] = userDic["sessionId"];//访问请求要添加sessionId头部文件变量
                // 单点登录验证
                var validResult = UserManager.ValidSessionId(userDic["uid"], userDic["sessionId"]);

                if (validResult != SysConstants.TokenValidType.Success)
                {
                    return validResult;
                }
                //当token过期时间小于8小时，更新token并重新返回新的token
                if (date.AddHours(-8) > DateTime.Now) return validResult;
                #region 滑动刷新Token

                var newSessionId = Guid.NewGuid().ToString("N");
                userDic["sessionId"] = newSessionId;
                var nToken = TokenManager.GenerateToken(userDic.ToJson());
                CurrentUser.SessionId = newSessionId;
                CurrentUser.UserToken = nToken;
                UserManager.CurrentUser = CurrentUser;
                GlobalSystemWeb.HttpContext.Response.Headers["uid"] = userDic["uid"];
                GlobalSystemWeb.HttpContext.Response.Headers["sessionId"] = userDic["sessionId"];
                GlobalSystemWeb.HttpContext.Response.Headers["token"] = nToken;
                GlobalSystemWeb.HttpContext.Response.Headers["Access-Control-Expose-Headers"] = "token";
                return validResult;

                #endregion
            }

            return SysConstants.TokenValidType.LogonInvalid;
        }
    }
}
