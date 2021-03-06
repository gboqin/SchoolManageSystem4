using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using SchoolManageSystem.Basics.Config;
using SchoolManageSystem.Basics.Enums;
using SchoolManageSystem.Basics.Exceptions;
using SchoolManageSystem.Basics.Extensions;
using SchoolManageSystem.Basics.ResponseResults;
using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolManageSystem.Common.Filter.Authorization
{
    /// <summary>
    /// 在所有需要授权验证上添加此属性
    /// 登录验证
    /// </summary>
    public class AuthorizationFilter : Attribute, IAuthorizationFilter
    {
        /// <summary>
        /// Called early in the filter pipeline to confirm request is authorized.
        /// 管道执行顺序 --》 中间件 - 权限filter - 资源filter - 异常filter - 模型绑定filter - 动作filter前 - 动作 - 动作filter后  - U型返回
        /// </summary>
        /// <param name="context">The <see cref="T:Microsoft.AspNetCore.Mvc.Filters.AuthorizationFilterContext" />.</param>
        public void OnAuthorization(AuthorizationFilterContext context)
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

            var validResult = UserManager.TokenLogin();

            switch (validResult)
            {
                case SysConstants.TokenValidType.LogonInvalid:
                    context.Result = new CustomHttpStatusCodeResult(200, ResponseCode.Unauthorized, validResult.GetDescription());
                    return;
                case SysConstants.TokenValidType.LoggedInOtherPlaces:
                    context.Result = new CustomHttpStatusCodeResult(200, ResponseCode.Unauthorized, validResult.GetDescription());
                    return;
                case SysConstants.TokenValidType.Success:
                    return;
                default:
                    throw new CustomSystemException("未实现的TokenValidType", ResponseCode.DataTypeError);
            }
        }
    }
}
