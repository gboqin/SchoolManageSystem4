using SchoolManageSystem.Common.Filter.Authorization;
using SchoolManageSystem.Dto.SystemBo;
using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolManageSystem.Common.Controllers
{
    /// <summary>
    /// 用户权限验证控制器
    /// </summary>
    [AuthorizationFilter]
    public abstract class BaseUserController : BaseController
    {
        private UserCacheBo _user;

        /// <summary>
        /// 当前用户
        /// </summary>
        protected UserCacheBo CurrentUser
        {
            get => _user ??= UserManager.CurrentUser;
            set
            {
                UserManager.CurrentUser = value;
                _user = value;
            }
        }

        /*public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            if (filterContext.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor)
            {
                var isDefined = controllerActionDescriptor.MethodInfo.GetCustomAttributes(true)
                    .Any(a => a.GetType() == typeof(AllowAnonymousAttribute));
                if (isDefined)
                {
                    return;
                }
            }

            var token = Request.Headers["token"];
            if (string.IsNullOrEmpty(token))
            {
                filterContext.Result = new CustomHttpStatusCodeResult(200, ResponseCode.Unauthorized, "未授权");
                return;
            }
            var str = TokenManager.ValidateToken(token, out DateTime date);
            if (!string.IsNullOrEmpty(str) || date > DateTime.Now)
            {
                //当token过期时间小于五小时，更新token并重新返回新的token
                if (date.AddHours(-5) > DateTime.Now) return;
                var nToken = TokenManager.GenerateToken(str);
                Response.Headers["token"] = nToken;
                Response.Headers["Access-Control-Expose-Headers"] = "token";
                return;
            }

            filterContext.Result = new CustomHttpStatusCodeResult(200, ResponseCode.Unauthorized, "未授权");
        }*/
    }
}
