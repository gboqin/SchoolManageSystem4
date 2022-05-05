using Microsoft.AspNetCore.Mvc.Filters;
using SchoolManageSystem.Basics.Enums;
using SchoolManageSystem.Basics.Exceptions;
using SchoolManageSystem.Basics.Extensions;
using SchoolManageSystem.Basics.ResponseResults;
using SchoolManageSystem.Common.NLog;
using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolManageSystem.Common.Filter.ExceptionFilter
{
    /// <inheritdoc />
    /// <summary>
    /// 全局异常处理过滤器
    /// </summary>
    public class ExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            if (filterContext.ExceptionHandled)
                return;

            var controllerName = (string)filterContext.RouteData.Values["controller"];
            var actionName = (string)filterContext.RouteData.Values["action"];
            var request = filterContext.HttpContext.Request;

            LogHelper.Logger.Fatal(filterContext.Exception,
                $"【异常信息】：{filterContext.Exception.Message}  【请求路径】：{request.Method}:{request.Path}\n " +
                $"【控制器】:{controllerName} - 【方法】:{actionName}\n " +
                $"【主机地址】:{ GlobalSystemWeb.GetClientIp()} " +
                $"【用户代理】:{ request.Headers["User-Agent"]}");//DemoWeb为前面章节添加的类

            if (filterContext.Exception is CustomSystemException se)
            {
                filterContext.Result = new CustomHttpStatusCodeResult(200, (ResponseCode)se.Code, se.Message);
            }
            else
            {
#if DEBUG
                Console.WriteLine(filterContext.Exception);
                var content = filterContext.Exception.ToJson();//ToJson为静态扩展方法 为前面章节添加的类
#else
                var content = "系统错误，请稍后再试或联系系统管理人员。";
#endif
                filterContext.Result = new CustomHttpStatusCodeResult(200, ResponseCode.UnknownEx, content);//CustomHttpStatusCodeResult为自定义返回结果 为前面章节添加的类
            }
            filterContext.ExceptionHandled = true;
        }
    }
}
