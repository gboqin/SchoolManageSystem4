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
                    //ȷ��NLog.config�������ַ�����appsettings.json��ͬ��
                    NLogExtensions.NlogConfig("NLog.config");
                    //��ʼ�����ݿ�
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
                .UseServiceProviderFactory(new AutofacServiceProviderFactory()) //�滻ԭ��DI ʹ��autofac
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                }).AddNlogService();//�滻ԭ����Log
    }
}
