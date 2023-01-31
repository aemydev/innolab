using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using UserService.Repositories;
using UserServiceModels;
using UserServiceRequestMessages;
using UserServiceResponseMessages;

namespace UserService.Consumers
{
    public class OneUserRequestConsumer : IConsumer<OneUserRequest>
    {
        private readonly ApplicationUserContext _usercontext;

        public OneUserRequestConsumer(ApplicationUserContext userContext)
        {
            _usercontext = userContext;
        }
        public async Task Consume(ConsumeContext<OneUserRequest> context)
        {
            await Task.Run( async() =>
            {
                 var res = _usercontext.Users.Single(u => u.Id == context.Message.UserId);

                 await context.RespondAsync<GetOneUserResponse>(new { User = res });
            });
        }
    }
}
