using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using SchoolManageSystem.Basics.Enums;
using SchoolManageSystem.Basics.Exceptions;
using SchoolManageSystem.Basics.ResponseResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchoolManageSystem.Common.Filter.Permission
{
    /// <summary>
    /// 权限验证 -- 判断当前操作用户 租户是否有操作权限 PermissionCacheFilter("d","e")ActionFilterAttribute
    /// </summary>
    //public class PermissionCacheFilterAttribute : Attribute, IActionFilter
    public class PermissionCacheFilterAttribute : ActionFilterAttribute
    {
        public PermissionCacheFilterAttribute()
        {
        }

        public string[] Codes { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="codes">权限组  以英文字符分割</param>
        public PermissionCacheFilterAttribute(params string[] codes)
        {
            Codes = codes;
        }
        /// <summary>
        /// Called after the action executes, before the action result.
        /// </summary>
        /// <param name="context">The <see cref="T:Microsoft.AspNetCore.Mvc.Filters.ActionExecutedContext" />.</param>
        public override void OnActionExecuted(ActionExecutedContext context)
        {
        }

        /// <summary>
        /// Called before the action executes, after model binding is complete.
        /// </summary>
        /// <param name="context">The <see cref="T:Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext" />.</param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!(context.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor))
            {
                return;
            }

            var isDefined = controllerActionDescriptor.MethodInfo.IsDefined(typeof(AllowAnonymousAttribute), true);
            if (isDefined)
            {
                return;
            }

            // ActionFilter在AuthorizationFilter之后执行，只要是有登录验证的，都能取到user
            var user = UserManager.CurrentUser;
            if (user == null)
            {
                throw new CustomSystemException("请先对需权限验证的方法加上登录验证", ResponseCode.Unauthorized);
            }

            if (Codes == null || Codes.Length == 0)
                return;
            var codes = new List<String>(Codes);
            //是否有交集
            if (user.MenuCodeList.Intersect(codes).Count() > 0)
            {
                return;
            }
            context.Result = new CustomHttpStatusCodeResult(200, ResponseCode.Unauthorized1, "无权限操作");
        }
    }
}
