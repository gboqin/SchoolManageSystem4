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
                //cookie��֤��������
                options.Cookie.Name = "AuthCookie";//cookie����
                options.LoginPath = "/Login/Login";//��¼·��
                options.LogoutPath = "/Home/index";//��¼·��
                options.Cookie.HttpOnly = true;//cookie����Ȩ��
                //options.SessionStore = new MemoryCacheTicketStore();
            });

            //������֤�������ˡ�auth1������,�ò�����һ���������Ҫ������ΪҪ��Ĳ����ṩ
            //services.AddAuthorization(option => {
            //    option.AddPolicy("auth1", policy => {
            //        policy.Requirements.Add(new AdultPolicyRequirement(12));
            //    });
            //});

            // ��������������Ȩ
            //services.AddAuthorization(option =>
            //{
            //    option.AddPolicy("cc", policy => policy.RequireClaim("cc"));
            //});

            // ���ڽ�ɫ���������Զ��������Ȩ
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

            // ���ڽ�ɫ���������Զ��������Ȩ
            //services.AddAuthorization(option => {
            //    option.AddPolicy("CustomPolicy", policy => {
            //        policy.RequireAssertion(
            //            context => context.User.IsInRole("admin")
            //            && context.User.HasClaim(claim => claim.Type == "hello" && claim.Value == "world")
            //            || context.User.IsInRole("superadmin"));
            //    });
            //});

            //��ɫ����
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

            ////�Զ��������Ȩ--�������Claim client_role & ������Admin
            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("AdminPolicy",
            //        policyBuilder => policyBuilder
            //        .RequireAssertion(context =>
            //        context.User.HasClaim(c => c.Type == "client_role")
            //        && context.User.Claims.First(c => c.Type.Equals("client_role")).Value.Equals("Admin")));
            //});
            ////�Զ�����Ȩ--�������Claim client_EMail & ����qq��β
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

            //��֤����˭��ע��˳��Ҫ�ŵ�UseAuthorization֮ǰ,�������Ǽ�顰����˭����
            app.UseAuthentication();

            //�Ƿ��������,�����ڼ�顰�Ƿ�������������Ϣ����
            app.UseAuthorization();

            app.UseCookiePolicy();

            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Login}/{action=Index}/{id?}");

                //���������Ĵ�ͳ·���������
                /* [Area("SysManager")]   demo  ������һ��Ҫ��          
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
