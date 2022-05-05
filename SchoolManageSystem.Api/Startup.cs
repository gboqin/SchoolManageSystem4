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
            // ֱ����Autofacע�������Զ���� 
            builder.RegisterModule(new CustomAutofacModule());
            // ע��redisʵ��
            builder.RegisterInstance(new RedisClient(Configuration)).SingleInstance().PropertiesAutowired();
            builder.RegisterInstance(new Common.Cache.Menory.MemoryCache("Cache")).SingleInstance().PropertiesAutowired();
        }

        //��������������Ϊ _myAllowSpecificOrigins�� ���������������
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(HtmlEncoder.Create(UnicodeRanges.All));
            //IHttpContextAccessor�������ӣ��������Լ�ע��,����
            //GlobalSystemWeb.Configure(app.ApplicationServices.GetRequiredService<IHttpContextAccessor>())����IHttpContextAccessorδע��
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddMvc(options =>
            {
                options.MaxModelValidationErrors = 5;//��֤����������
                //options.AllowValidatingTopLevelNodes = true;//�Ƿ�������֤�����ڵ�  �ӿڷ�������
                options.Filters.Add(new ExceptionFilter());//����쳣���������
            }).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            services.AddHttpClient<IHttpClientBuilder>();
            //����Cors
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

            //ע�Ỻ��
            services.AddMemoryCache();

            //ע�Ṥ����Ԫ��ͬʱע����DBContext��
            services.AddUnitOfWorkService<SchoolDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("SqlServerConnection")));
            
            //���AutoMapperע��
            //services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);
            // ��� AutoMapper ӳ���ϵ
            services.AddAutoMapper(c => c.AddProfile<AutoMapperProfiles>());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IMemoryCache memoryCache)
        {
            //autofac ���� 
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

            //�������
            //app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().WithExposedHeaders("Content-Disposition"));ֻ����Ie���á�Egde/google����
            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());//.WithExposedHeaders("Access-Control-Expose-Headers")

            app.UseRouting();

            //ע�����
            app.UseCors(MyAllowSpecificOrigins);

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
