﻿/******************************************
 * AUTHOR:          Rector
 * CREATEDON:       2018-09-26
 * OFFICIAL_SITE:    码友网(https://codedefault.com)--专注.NET/.NET Core
 * 版权所有，请勿删除
 ******************************************/

using AutoMapper;
using DncZeus.Api.Auth;
using DncZeus.Api.Entities;
using DncZeus.Api.Extensions;
using DncZeus.Api.Extensions.AuthContext;
using DncZeus.Api.Extensions.CustomException;
using DncZeus.Api.Job;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.WebEncoders;
using Microsoft.OpenApi.Models;
using Quartz;
using System;
using System.IO;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Unicode;


namespace DncZeus.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            
            var withOrigins = Configuration.GetSection("WithOrigins").Get<string[]>();
            services.AddCors(o =>
                o.AddPolicy("CorsPolicy",
                    builder => builder
                        .WithOrigins(withOrigins)
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        //.AllowAnyOrigin()
                        .AllowCredentials()
                ));

            services.AddMemoryCache();
            services.AddHttpContextAccessor();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppAuthenticationSettings>(appSettingsSection);
            // JWT
            var appSettings = appSettingsSection.Get<AppAuthenticationSettings>();
            services.AddJwtBearerAuthentication(appSettings);
            services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
            services.AddAutoMapper(typeof(Startup));

            services.Configure<WebEncoderOptions>(options =>
                options.TextEncoderSettings = new TextEncoderSettings(UnicodeRanges.BasicLatin, UnicodeRanges.CjkUnifiedIdeographs)
            );

            //services
            //    .AddMvc(config =>
            //    {
            //        //config.Filters.Add(new ValidateModelAttribute());
            //    })
            //    .AddJsonOptions(options =>
            //    {
            //        options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            //    })
            //    .SetCompatibilityVersion(CompatibilityVersion.Latest);

            services.AddControllers().AddNewtonsoftJson();

            services.AddDbContext<DncZeusDbContext>(options =>
                options.UseLazyLoadingProxies()//启用懒加载
                       .UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
                       //.UseLoggerFactory()
                // 如果使用SQL Server 2008数据库，请添加UseRowNumberForPaging的选项
                // 参考资料:https://github.com/aspnet/EntityFrameworkCore/issues/4616 
                // 【重要说明】:2020-03-23更新，微软官方最新的Entity Framework Core已不再支持UseRowNumberForPaging()，请尽量使用SQL Server 2012 +版本
                //options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),b=>b.UseRowNumberForPaging())
                );
            // Add QuartzJob
            services.AddQuartzJob();

            services.AddHttpClient();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "RBAC Management System API", Version = "v1" });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            // 注入日志
            services.AddLogging(config =>
            {
                config.AddLog4Net();
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="lifetime"></param>
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime lifetime)
        {
            lifetime.ApplicationStarted.Register(() =>
            {
                Console.WriteLine("正在初始化人脸识别引擎...");
                FaceServer.Init(Convert.ToDouble(Configuration.GetSection("Tolerance").Value));
            });
            lifetime.ApplicationStopped.Register(() => {
                FaceServer.Dispose();
                Console.WriteLine("已释放人脸识别引擎");
                //Console.ReadLine();
            });
            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
            }


            app.UseDeveloperExceptionPage();
            //app.UseExceptionHandler("/error/500");
            //app.UseStatusCodePagesWithReExecute("/error/{0}");

            app.UseStaticFiles();
            app.UseFileServer();
            app.UseAuthentication();
            app.UseCors("CorsPolicy");
            app.ConfigureCustomExceptionMiddleware();

            var serviceProvider = app.ApplicationServices;
            var httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();
            AuthContextService.Configure(httpContextAccessor);
            WeChatApi.Configure(serviceProvider);

            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(ep =>
            {
                ep.MapControllerRoute(name: "areaRoute", pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                ep.MapControllerRoute(name: "apiDefault", pattern: "api/{controller=Home}/{action=Index}/{id?}");
                ep.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            string dest = Path.Combine(Directory.GetCurrentDirectory(), $"upload/files");
            if (!Directory.Exists(dest))
            {
                Directory.CreateDirectory(dest);
            }
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(dest),
                RequestPath = new PathString("/src")
            });

            //app.UseMvc(routes =>
            //{

            //    routes.MapRoute(
            //         name: "areaRoute",
            //         template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

            //    routes.MapRoute(
            //        name: "apiDefault",
            //        template: "api/{controller=Home}/{action=Index}/{id?}");

            //    routes.MapRoute(
            //        name: "default",
            //        template: "{controller=Home}/{action=Index}/{id?}");
            //});
            if (Configuration.GetSection("UseSwagger").Value == "true")
            {
                app.UseSwagger(o =>
                {
                //o.PreSerializeFilters.Add((document, request) =>
                //{
                //    document.Paths = document.Paths.ToDictionary(p => p.Key.ToLowerInvariant(), p => p.Value);
                //});
                });
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "RBAC API V1");
                //c.RoutePrefix = "";
                });
            }
        }
    }
}
