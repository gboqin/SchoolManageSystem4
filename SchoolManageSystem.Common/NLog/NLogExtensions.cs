using Microsoft.Extensions.Hosting;
//手动添加引用
using Microsoft.Extensions.Logging;
using NLog.Web;
using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolManageSystem.Common.NLog
{
    /// <summary>
    /// 必需安装Microsoft.EntityFrameworkCore.SqlServer包
    /// </summary>
    public static class NLogExtensions
    {
        public static void NlogConfig(string nlogPath)
        {
            NLogBuilder.ConfigureNLog(nlogPath);
        }

        public static IHostBuilder AddNlogService(this IHostBuilder builder)
        {
            return builder
                  .ConfigureLogging(logging =>
                  {
                      //从builder移除内置的日志框架（如果引用第三方，一定要设置这属性）
                      logging.ClearProviders();
                      logging.SetMinimumLevel(LogLevel.Information);
                      //将日志添加到控制台
                      logging.AddConsole();
                  }).UseNLog();// 替换,NLog作为日志管理//引用第三方日志框架;其中，UseNLog是拓展方法，需要引入NLog.Web.AspNetCore
        }
    }
}
