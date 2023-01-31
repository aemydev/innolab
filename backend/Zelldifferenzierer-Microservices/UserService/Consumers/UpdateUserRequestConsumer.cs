using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using UserServiceRequestMessages;

namespace UserService.Consumers
{
    public class UpdateUserRequestConsumer : IConsumer<UpdateUserRequest>
    {
        public Task Consume(ConsumeContext<UpdateUserRequest> context)
        {
            throw new NotImplementedException();
        }
    }
}
