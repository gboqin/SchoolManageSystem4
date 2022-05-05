using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SchoolManageSystem.Application.Autofac;
using SchoolManageSystem.Common;
using SchoolManageSystem.Common.AutoMapper;
using SchoolManageSystem.Common.Cache.Redis;
using SchoolManageSystem.Common.Filter.ExceptionFilter;
using SchoolManageSystem.Repositorys;
using SchoolManageSystem.Repositorys.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace SchoolManageSystem.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            GlobalSystemWeb.Configuration = configuration;

            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            // 直接用Autofac注册我们自定义的 
            builder.RegisterModule(new CustomAutofacModule());
            // 注册redis实例
            builder.RegisterInstance(new RedisClient(Configuration)).SingleInstance().PropertiesAutowired();
            builder.RegisterInstance(new Common.Cache.Menory.MemoryCache("Cache")).SingleInstance().PropertiesAutowired();
        }

        //将策略名称设置为 _myAllowSpecificOrigins。 策略名称是任意的
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(HtmlEncoder.Create(UnicodeRanges.All));
            //IHttpContextAccessor不再连接，您必须自己注册,否则
            //GlobalSystemWeb.Configure(app.ApplicationServices.GetRequiredService<IHttpContextAccessor>())报错：IHttpContextAccessor未注册
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddMvc(options =>
            {
                options.MaxModelValidationErrors = 5;//验证错误最大个数
                //options.AllowValidatingTopLevelNodes = true;//是否允许验证顶级节点  接口方法参数
                options.Filters.Add(new ExceptionFilter());//添加异常处理过滤器
            }).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            services.AddHttpClient<IHttpClientBuilder>();
            //配置Cors
            services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                                  builder =>
                                  {
                                      builder.WithOrigins("http://localhost:32370",
                                                          "http://localhost:32371");
                                  });
                /*       http://localhost:32370/               
                builder.WithOrigins(corsUrl)
                .WithMethods("PUT", "DELETE", "GET","POST","PATCH")
                .SetIsOriginAllowed((host) => true)
                        .AllowAnyMethod()
                        .AllowCredentials()
                        .AllowAnyHeader(); 
                 */
            });

            services.AddControllers();

            //注册缓存
            services.AddMemoryCache();

            //注册工作单元（同时注册了DBContext）
            services.AddUnitOfWorkService<SchoolDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("SqlServerConnection")));
            
            //添加AutoMapper注册
            //services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);
            // 添加 AutoMapper 映射关系
            services.AddAutoMapper(c => c.AddProfile<AutoMapperProfiles>());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IMemoryCache memoryCache)
        {
            //autofac 新增 
            GlobalSystemWeb.AutofacContainer = app.ApplicationServices.CreateScope().ServiceProvider.GetAutofacRoot();

            //GlobalSystemWeb.Configure(app.ApplicationServices.GetRequiredService<IHttpContextAccessor>());
            using (var container = GlobalSystemWeb.AutofacContainer.BeginLifetimeScope())
            {
                GlobalSystemWeb.Configure(container.Resolve<IHttpContextAccessor>());
            }

            GlobalSystemWeb.Environment = env;

            GlobalSystemWeb.MemoryCache = memoryCache;

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //允许跨域
            //app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().WithExposedHeaders("Content-Disposition"));只能在Ie调用、Egde/google不行
            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());//.WithExposedHeaders("Access-Control-Expose-Headers")

            app.UseRouting();

            //注册跨域
            app.UseCors(MyAllowSpecificOrigins);

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
