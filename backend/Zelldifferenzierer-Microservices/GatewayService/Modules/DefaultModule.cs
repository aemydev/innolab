using System;
using Autofac;
using GatewayService.Validators;
using GreenPipes;
using LogServiceRequestMessages;
using LogServiceResponseMessages;
using MassTransit;
using MassTransit.Clients;
using Polly;
using UserServiceRequestMessages;
using UserServiceResponseMessages;

namespace GatewayService.Modules
{

    public class DefaultModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            MultiValidatorBase.Validators.Add(typeof(SqlInjectionValidator<>));

            var timeout = TimeSpan.FromSeconds(10);

            builder.Register(c => Bus.Factory.CreateUsingRabbitMq(cfg =>
                    cfg.Host(new Uri(Startup.Configuration["RabbitMqUri"]), h =>
                    {
                        c = c.Resolve<IComponentContext>();
                        cfg.UseRetry(configurator => configurator.Interval(5, TimeSpan.FromMilliseconds(500)));
                        cfg.PrefetchCount = 50;
                        cfg.UseCircuitBreaker(cb =>
                        {
                            cb.TrackingPeriod = TimeSpan.FromMinutes(1);
                            cb.TripThreshold = 15;
                            cb.ActiveThreshold = 20;
                            cb.ResetInterval = TimeSpan.FromMinutes(4);
                        });
                        h.Username(Startup.Configuration["RabbitMqUser"]);
                        h.Password(Startup.Configuration["RabbitMqPassword"]);
                    })
                ))
                .As<IBusControl>()
                .As<IPublishEndpoint>()
                .SingleInstance();
        }
    }
}
