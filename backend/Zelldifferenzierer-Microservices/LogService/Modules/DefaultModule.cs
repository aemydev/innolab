using System;
using Autofac;
using LogService.Consumers;
using LogService.Repositories;
using MassTransit;
using GreenPipes;

namespace LogService.Modules
{
    /// <inheritdoc />
    /// <summary>
    /// This Module will be injected in the service Startup.cs
    /// Types are registered to the container
    /// RabbitBus is registered -> QueueName from file
    /// </summary>
    public class DefaultModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //builder.Register(c => new LogServiceContext())
            //    .AsSelf();

            //builder.Register(c => new LogServiceRepository(c.Resolve<LogServiceContext>())).AsSelf();


            //builder.RegisterType<LogRequestConsumer>();
            //builder.RegisterType<GetAllLogsRequestConsumer>();
            //builder.RegisterType<GetLogsByDateAfterConsumer>();
            //builder.RegisterType<GetLogsByDateBeforeConsumer>();
            //builder.RegisterType<GetLogsByDateBetweenConsumer>();
            //builder.RegisterType<GetLogsByLevelRequestConsumer>();

            //builder.Register(c => Bus.Factory.CreateUsingRabbitMq(cfg =>
            //    {
            //        //cfg.UseSerilog();
            //        c = c.Resolve<IComponentContext>();
            //        var host = cfg.Host(new Uri(Startup.Configuration["RabbitMqUri"]), h =>
            //        {
            //            cfg.UseSerilog(); // Audit with global logger
            //            //cfg.UseSerilog(logger); // Audit with custom logger for bus

            //            //RetryPattern Configuration
            //            cfg.UseRetry(configurator => configurator.Interval(5, TimeSpan.FromMilliseconds(500)));

            //            //CircuitBreaker Configuration
            //            cfg.UseCircuitBreaker(cb =>
            //            {
            //                cb.TrackingPeriod = TimeSpan.FromMinutes(1);
            //                cb.TripThreshold = 15; //Trips if more than 15% of all messages fail
            //                cb.ActiveThreshold = 20; //Min messages before tripping can occur
            //                cb.ResetInterval = TimeSpan.FromMinutes(4); //recovery time
            //            });

            //            h.Username(Startup.Configuration["RabbitMqUser"]);
            //            h.Password(Startup.Configuration["RabbitMqPassword"]);

            //        });

            //        cfg.ReceiveEndpoint(host, Startup.Configuration["LogQueue"], ep =>
            //        {
            //            ep.Consumer(typeof(LogRequestConsumer), c.Resolve);
                        
            //        });

            //        cfg.ReceiveEndpoint(host, Startup.Configuration["GetAllLogsQueue"], ep =>
            //        {
            //            ep.Consumer(typeof(GetAllLogsRequestConsumer), c.Resolve);
            //        });

            //        cfg.ReceiveEndpoint(host, Startup.Configuration["GetLogsByDateAfterQueue"], ep =>
            //        {
            //            ep.Consumer(typeof(GetLogsByDateAfterConsumer), c.Resolve);
            //        });

            //        cfg.ReceiveEndpoint(host, Startup.Configuration["GetLogsByDateBeforeQueue"], ep =>
            //        {
            //            ep.Consumer(typeof(GetLogsByDateBeforeConsumer), c.Resolve);
            //        });

            //        cfg.ReceiveEndpoint(host, Startup.Configuration["GetLogsByDateBetweenQueue"], ep =>
            //        {
            //            ep.Consumer(typeof(GetLogsByDateBetweenConsumer), c.Resolve);
            //        });

            //        cfg.ReceiveEndpoint(host, Startup.Configuration["GetLogsByLevelQueue"], ep =>
            //        {
            //            ep.Consumer(typeof(GetLogsByLevelRequestConsumer), c.Resolve);
            //        });
            //    }))
            //    .As<IBusControl>()
            //    .As<IPublishEndpoint>()
            //    .SingleInstance();

        }
    }

}