using System;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using UserService.Repositories;
using UserServiceModels;
using UserServiceRequestMessages;
using UserServiceResponseMessages;

namespace UserService.Consumers
{
    public class DeleteUserRequestConsumer : IConsumer<DeleteUserRequest>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public DeleteUserRequestConsumer(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task Consume(ConsumeContext<DeleteUserRequest> context)
        {
            try
            {
                  await _userManager.DeleteAsync(await _userManager.FindByIdAsync(context.Message.UserId));
                  await context.RespondAsync<SuccessResponse>(new {Success = true});
            }
            catch (Exception e)
            {
                Serilog.Log.Error($"Failed to delete User {context.Message.UserId} was requested, Exception: {e}");
            }

        }
    }
}
