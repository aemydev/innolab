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
    public class GetLogsByDateBeforeConsumer : IConsumer<GetLogsByDateBefore>
    {
        private readonly LogServiceRepository _repository;

        public GetLogsByDateBeforeConsumer(LogServiceRepository repository)
        {
            _repository = repository;
        }

        public async Task Consume(ConsumeContext<GetLogsByDateBefore> context)
        {
            try
            {
                var res = _repository.GetLogsByDateBefore(context.Message.Date);
                await context.RespondAsync<LogListResponse>(res);
            }
            catch (Exception e)
            {
                Log.Error($"GetLogsByDateBeforeConsumer threw an exception! Exception: {e}");
            }

        }
    }
}
