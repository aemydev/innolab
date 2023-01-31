using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LogService.Repositories;
using LogServiceRequestMessages;
using LogServiceResponseMessages;
using MassTransit;
using Serilog;

namespace LogService.Consumers
{
    public class GetLogsByDateBetweenConsumer : IConsumer<GetLogsByDateBetween>
    {
        private readonly LogServiceRepository _repository;

        public GetLogsByDateBetweenConsumer(LogServiceRepository repository)
        {
            _repository = repository;
        }

        public async Task Consume(ConsumeContext<GetLogsByDateBetween> context)
        {
            try
            {
                var res = _repository.GetLogsByDateBetween(context.Message.DateAfter, context.Message.DateBefore);
                await context.RespondAsync<LogListResponse>(res);
            }
            catch (Exception e)
            {
                Log.Error($"GetLogsByDateBetweenConsumer threw an exception! Exception: {e}");
            }

        }
    }
}
