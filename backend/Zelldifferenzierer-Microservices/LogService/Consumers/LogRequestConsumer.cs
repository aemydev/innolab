using System;
using System.Threading.Tasks;
using LogService.Repositories;
using LogServiceModels;
using LogServiceRequestMessages;
using MassTransit;
using Serilog;

namespace LogService.Consumers
{
    public class LogRequestConsumer : IConsumer<LogRequest>
    {
        private readonly LogServiceRepository _logRepository;
        
        public LogRequestConsumer(LogServiceRepository logRepository)
        {
            _logRepository = logRepository;
        }

        /// <summary>
        /// Consumes LogRequests and Logs them into the Console, File, DB
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task Consume(ConsumeContext<LogRequest> request)
        {
            var entry = new LogEntry(request.Message.LogMessage, request.Message.Sender, request.SentTime,
                request.Message.Level);
            await _logRepository.AddAsync(entry);
            await _logRepository.SaveAsync();

            switch (request.Message.Level)
            {
                case ELevel.Information:
                    Log.Information($"{entry}");
                    break;
                case ELevel.Warning:
                    Log.Warning($"{entry}");
                    break;
                case ELevel.Error:
                    Log.Error($"{entry}");
                    break;
                case ELevel.Fatal:
                    Log.Fatal($"{entry}");
                    break;
                default:
                    Log.Fatal($"{entry}");
                    break;
            }

            //TODO REMOVE -> JUST FOR DEBUGGING
            Log.Information($"LogRequestConsumer consumed a message at {DateTime.Now}");
            Log.Information(request.ToString());
        }
    }
}
