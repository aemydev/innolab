using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using LogService.Consumers;
using LogService.Modules;
using LogService.Repositories;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Serilog;
using Serilog.Events;
using TaskUtil = MassTransit.Util.TaskUtil;

namespace LogService
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
                .WriteTo.File("..\\Logs\\GatewayService\\LogService-{Date}.txt")
                .WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Debug)
                .CreateLogger();
        }

        public static IConfiguration Configuration { get; set; }


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddHttpContextAccessor();
            services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();

            services.AddCors(o => o.AddPolicy("CorsPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));

            services.AddDbContext<LogServiceContext>();

            services.AddScoped<LogServiceRepository>();

            services.AddMassTransit(x =>
            {
                x.AddConsumer(typeof(GetAllLogsRequestConsumer));
                x.AddConsumer(typeof(GetLogsByDateAfterConsumer));
                x.AddConsumer(typeof(GetLogsByDateBeforeConsumer));
                x.AddConsumer(typeof(GetLogsByDateBetweenConsumer));
                x.AddConsumer(typeof(GetLogsByLevelRequestConsumer));
                x.AddConsumer(typeof(LogRequestConsumer));

                //x.SetKebabCaseEndpointNameFormatter();

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


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IBusControl bus, LogServiceContext context)
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
            //app.UseEndpoints(endpoints => {
            //    endpoints.MapControllers();
            //});

            app.UseCookiePolicy();

            context.Database.EnsureCreated();

            var busHandle = TaskUtil.Await(() => bus.StartAsync());

        }
    }
}
