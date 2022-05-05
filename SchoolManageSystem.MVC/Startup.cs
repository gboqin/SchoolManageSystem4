using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolManageSystem.MVC
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(10);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            services.AddSession();
            services.AddSingleton<ITempDataProvider, CookieTempDataProvider>();

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie(options =>
            {
                //cookie认证更多配置
                options.Cookie.Name = "AuthCookie";//cookie名称
                options.LoginPath = "/Login/Login";//登录路径
                options.LogoutPath = "/Home/index";//登录路径
                options.Cookie.HttpOnly = true;//cookie操作权限
                //options.SessionStore = new MemoryCacheTicketStore();
            });

            //策略认证，创建了“auth1”策略,该策略有一个最低年龄要求，其作为要求的参数提供
            //services.AddAuthorization(option => {
            //    option.AddPolicy("auth1", policy => {
            //        policy.Requirements.Add(new AdultPolicyRequirement(12));
            //    });
            //});

            // 基于声明策略授权
            //services.AddAuthorization(option =>
            //{
            //    option.AddPolicy("cc", policy => policy.RequireClaim("cc"));
            //});

            // 基于角色和声明的自定义策略授权
            services.AddAuthorization(option =>
            {
                option.AddPolicy("SystemPolicy", policy =>
                {
                    policy.RequireAssertion(context => context.User.IsInRole("System"));
                });
            });
            services.AddAuthorization(option =>
            {
                option.AddPolicy("AdminPolicy", policy =>
                {
                    policy.RequireAssertion(context => context.User.IsInRole("Admin"));
                });
            });
            services.AddAuthorization(option =>
            {
                option.AddPolicy("CustomPolicy", policy =>
                {
                    policy.RequireAssertion(context => context.User.IsInRole("Custom"));
                });
            });

            // 基于角色和声明的自定义策略授权
            //services.AddAuthorization(option => {
            //    option.AddPolicy("CustomPolicy", policy => {
            //        policy.RequireAssertion(
            //            context => context.User.IsInRole("admin")
            //            && context.User.HasClaim(claim => claim.Type == "hello" && claim.Value == "world")
            //            || context.User.IsInRole("superadmin"));
            //    });
            //});

            //角色策略
            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("RequireAdministratorRole",
            //         policy => policy.RequireRole("Administrator"));
            //});
            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("ElevatedRights", policy =>
            //          policy.RequireRole("Administrator", "PowerUser", "BackupAdministrator"));
            //});

            ////自定义策略授权--必须包含Claim client_role & 必须是Admin
            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("AdminPolicy",
            //        policyBuilder => policyBuilder
            //        .RequireAssertion(context =>
            //        context.User.HasClaim(c => c.Type == "client_role")
            //        && context.User.Claims.First(c => c.Type.Equals("client_role")).Value.Equals("Admin")));
            //});
            ////自定义授权--必须包含Claim client_EMail & 必须qq结尾
            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("EMailPolicy",
            //        policyBuilder => policyBuilder
            //        .RequireAssertion(context =>
            //        context.User.HasClaim(c => c.Type == "client_EMail")
            //        && context.User.Claims.First(c => c.Type.Equals("client_EMail")).Value.EndsWith("@qq.com")));
            //});

            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                .AddSessionStateTempDataProvider();
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            //验证你是谁，注意顺序，要放到UseAuthorization之前,帮助我们检查“您是谁？”
            app.UseAuthentication();

            //是否允许访问,有助于检查“是否允许您访问信息？”
            app.UseAuthorization();

            app.UseCookiePolicy();

            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Login}/{action=Index}/{id?}");

                //将控制器的传统路由添加区域
                /* [Area("SysManager")]   demo  这两句一定要加          
                   [Route("SysManager/[controller]/[action]")]*/
                endpoints.MapAreaControllerRoute(
                     name: "areas",
                     areaName: "areas",
                     pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                /*
                 endpoints.MapAreaControllerRoute(
                     name: "areas", 
                     areaName: "SysManager",
                     pattern: "SysManager/{controller=Home}/{action=Index}/{id?}");                
                 */
            });
        }
    }
}
