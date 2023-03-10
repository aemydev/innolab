using System;
using System.Text;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using GatewayService.Modules;
using GreenPipes.Util;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Events;
using TaskUtil = MassTransit.Util.TaskUtil;

namespace GatewayService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            //sets Configuration to file named ApplicationSettings -> QueueNames, Rabbit User,PW
            Configuration = Configuration.GetSection("ApplicationSettings");

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File("..\\Logs\\GatewayService\\GatewayLog-{Date}.txt")
                .WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Debug)
                .CreateLogger();
        }

        public static IConfiguration Configuration { get; set; }


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();

            services.AddHttpContextAccessor();
            services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();

            ////AuthPolicy creation 
            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("AdministratorsOnly", policy => policy.RequireRole("Administrator"));
            //});

            services.AddCors(o => o.AddPolicy("CorsPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));

            services.AddControllers();

            //JWT Authentication
            //var key = Encoding.ASCII.GetBytes(Configuration["Secret"]);
            //services.AddAuthentication(x =>
            //    {
            //        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //    })
            //    .AddJwtBearer(x =>
            //    {
            //        x.RequireHttpsMetadata = false;
            //        x.SaveToken = true;
            //        x.TokenValidationParameters = new TokenValidationParameters
            //        {
            //            ValidateIssuerSigningKey = true,
            //            IssuerSigningKey = new SymmetricSecurityKey(key),
            //            ValidateIssuer = false,
            //            ValidateAudience = false
            //        };
            //    });

            //Autofac container
            //var containerBuilder = new ContainerBuilder();
            //containerBuilder.RegisterModule<DefaultModule>();
            //containerBuilder.Populate(services);
            //var container = containerBuilder.Build();
            //return new AutofacServiceProvider(container);

            services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.UseRetry(configurator => configurator.Interval(5, TimeSpan.FromMilliseconds(500)));
                    cfg.PrefetchCount = 50;
                    cfg.UseCircuitBreaker(cb =>
                    {
                        cb.TrackingPeriod = TimeSpan.FromMinutes(1);
                        cb.TripThreshold = 15;
                        cb.ActiveThreshold = 20;
                        cb.ResetInterval = TimeSpan.FromMinutes(4);
                    });
                    cfg.Host("localhost", "/", h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });

                    cfg.ConfigureEndpoints(context);
                });
            });
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IBusControl bus)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }


            //app.UseHttpsRedirection();
            app.UseDefaultFiles();
            app.UseStaticFiles();
 
            app.UseRouting();
            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });

            app.UseCookiePolicy();

            var busHandle = TaskUtil.Await(() => bus.StartAsync());
        }
    }
}
