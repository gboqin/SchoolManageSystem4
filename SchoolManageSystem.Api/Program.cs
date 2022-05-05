using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SchoolManageSystem.Common.NLog;
using SchoolManageSystem.IRepositorys;
using SchoolManageSystem.Repositorys.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolManageSystem.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //CreateHostBuilder(args).Build().Run();
            try
            {
                var host = CreateHostBuilder(args).Build();
                using (IServiceScope scope = host.Services.CreateScope())
                {
                    //确保NLog.config中连接字符串与appsettings.json中同步
                    NLogExtensions.NlogConfig("NLog.config");
                    //初始化数据库
                    SeedData.Initialize(scope.ServiceProvider.GetRequiredService<IUnitOfWork<SchoolDbContext>>());
                }
                host.Run();
            }
            catch (Exception ex)
            {
                LogHelper.Logger.Error(ex.Message);
                throw;
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory()) //替换原生DI 使用autofac
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                }).AddNlogService();//替换原生的Log
    }
}
