using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using UserService.Repositories;
using UserServiceRequestMessages;
using UserServiceResponseMessages;

namespace UserService.Consumers
{
    public class AllUsersRequestConsumer : IConsumer<GetAllUsersRequest>
    {
        private readonly ApplicationUserContext _userContext;

        public AllUsersRequestConsumer(ApplicationUserContext userContext)
        {
            _userContext = userContext;
        }

        public async Task Consume(ConsumeContext<GetAllUsersRequest> context)
        {
            var res = await _userContext.Users.ToListAsync();

            await context.RespondAsync<GetAllUsersResponse>(new {Users = res});

        }
    }
}
