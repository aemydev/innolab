using System;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Serilog;
using UserServiceModels;
using UserServiceModels.Exceptions;
using UserServiceRequestMessages;
using UserServiceResponseMessages;

namespace UserService.Consumers
{
    public class RegisterRequestConsumer : IConsumer<RegisterRequest>
    {
        private readonly UserManager<ApplicationUser> _manager;
        public RegisterRequestConsumer(UserManager<ApplicationUser> manager)
        {
            _manager = manager;
        }

        public async Task Consume(ConsumeContext<RegisterRequest> context)
        {
            try
            {
                var newUser = new ApplicationUser(context.Message.Title, context.Message.Firstname,
                    context.Message.Lastname, context.Message.Email, context.Message.IdentificationNumber);

                if (await _manager.FindByEmailAsync(context.Message.Email) != null)
                {
                    await context.RespondAsync<RegisterResponse>(new { Success = false });
                    Log.Information($"Registration failed to to duplicated entries of {context.Message.Email} ");
                    throw new EmailAlreadyExistsException("Registration failed");
                }

                var res = await _manager.CreateAsync(newUser, context.Message.Password);
                if (res.Succeeded)
                {
                    await context.RespondAsync<RegisterResponse>(new { Success = true });
                }
                else
                {
                    await context.RespondAsync<RegisterResponse>(new { Success = false });
                }
                

            }
            catch (Exception e)
            {
                Serilog.Log.Error("Registration Failed");
                throw new RegistrationFailedException("Registration Failed", e);
            }
        }
    }
}
