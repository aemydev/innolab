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
    public class GetLogsByLevelRequestConsumer : IConsumer<GetLogsByLevelRequest>
    {
        private readonly LogServiceRepository _repository;

        public GetLogsByLevelRequestConsumer(LogServiceRepository repository)
        {
            _repository = repository;
        }

        public async Task Consume(ConsumeContext<GetLogsByLevelRequest> context)
        {
            try
            {
                var res = _repository.GetLogsByLogLevel(context.Message.Level);
                await context.RespondAsync<LogListResponse>(res);
            }
            catch (Exception e)
            {
                Log.Error($"GetLogsByLevelRequestConsumer threw an exception! Exception: {e}");
            }

        }
    }
}
