using System;
using Autofac;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using UserService.Consumers;
using UserService.Repositories;
using UserServiceModels;

namespace UserService.Modules
{
    public class DefaultModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //builder.RegisterType<UserManager<ApplicationUser>>().AsSelf().SingleInstance();
            //builder.RegisterType<UserStore<ApplicationUser>>().As<IUserStore<ApplicationUser>>();

            //builder.RegisterType<LoginRequestConsumer>().AsSelf();
            //builder.RegisterType<RegisterRequestConsumer>().AsSelf();
            //builder.RegisterType<AllUsersRequestConsumer>().AsSelf();
            //builder.RegisterType<DeleteUserRequestConsumer>().AsSelf();
            //builder.RegisterType<OneUserRequestConsumer>().AsSelf();
            //builder.RegisterType<UpdateUserRequestConsumer>().AsSelf();
            //builder.RegisterType<CreateAdminConsumer>().AsSelf();


            //builder.Register(c => Bus.Factory.CreateUsingRabbitMq(cfg =>
            //    {
            //        cfg.UseSerilog();
            //        c = c.Resolve<IComponentContext>();
            //        var host = cfg.Host(new Uri(Startup.Configuration["RabbitMqUri"]), h =>
            //        {
                        
            //            h.Username(Startup.Configuration["RabbitMqUser"]);
            //            h.Password(Startup.Configuration["RabbitMqPassword"]);

            //        });

            //        cfg.ReceiveEndpoint(host, Startup.Configuration["UserLoginQueue"],
            //            ep =>
            //            {
            //                ep.Consumer(typeof(LoginRequestConsumer), c.Resolve); 

            //            });

            //        cfg.ReceiveEndpoint(host, Startup.Configuration["UserRegistrationQueue"],
            //            ep =>
            //            {
            //                ep.Consumer(typeof(RegisterRequestConsumer), c.Resolve);
            //            });

            //        cfg.ReceiveEndpoint(host, Startup.Configuration["AllUsersRequestQueue"],
            //            ep =>
            //            {
            //                ep.Consumer(typeof(AllUsersRequestConsumer), c.Resolve);
            //            });

            //        cfg.ReceiveEndpoint(host, Startup.Configuration["OneUserRequestQueue"], ep =>
            //        {
            //            ep.Consumer(typeof(OneUserRequestConsumer), c.Resolve);
            //        });

            //        cfg.ReceiveEndpoint(host, Startup.Configuration["DeleteUserQueue"], ep =>
            //        {
            //            ep.Consumer(typeof(DeleteUserRequestConsumer), c.Resolve);
            //        });

            //        cfg.ReceiveEndpoint(host, Startup.Configuration["UpdateUserQueue"], ep =>
            //        {
            //            ep.Consumer(typeof(UpdateUserRequestConsumer), c.Resolve);
            //        });
                    
            //        cfg.ReceiveEndpoint(host, Startup.Configuration["AdminQueue"], ep =>
            //        {
            //            ep.Consumer(typeof(CreateAdminConsumer), c.Resolve);
            //        });

            //    }))
            //    .As<IBusControl>()
            //    .As<IPublishEndpoint>()
            //    .SingleInstance();

        
        }
    }
}
