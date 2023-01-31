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
    public class GetLogsByDateAfterConsumer : IConsumer<GetLogsByDateAfter>
    {
        private readonly LogServiceRepository _repository;

        public GetLogsByDateAfterConsumer(LogServiceRepository repository)
        {
            _repository = repository;
        }

        public async Task Consume(ConsumeContext<GetLogsByDateAfter> context)
        {
            try
            {
                var res = _repository.GetLogsByDateAfter(context.Message.Date);
                await context.RespondAsync<LogListResponse>(res);
            }
            catch (Exception e)
            {
                Log.Error($"GetLogsByDateAfterConsumer threw an exception! Exception: {e}");
            }

        }
    }
}
