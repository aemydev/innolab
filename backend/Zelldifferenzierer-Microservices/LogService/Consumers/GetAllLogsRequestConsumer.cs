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
    public class GetAllLogsRequestConsumer : IConsumer<GetAllLogsRequest>
    {
        private readonly LogServiceRepository _repository;

        public GetAllLogsRequestConsumer(LogServiceRepository repository)
        {
            _repository = repository;
        }

        public async Task Consume(ConsumeContext<GetAllLogsRequest> context)
        {
            try
            {
                var res = await _repository.GetAllAsync();
                await context.RespondAsync<LogListResponse>(new { Entries  =  res});
            }
            catch (Exception e)
            {
                Log.Error($"GetAllLogsConsumer threw an exception! Exception: {e}");
            }

        }
    }
}
