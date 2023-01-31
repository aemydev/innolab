using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LogServiceModels;
using Serilog;

namespace LogService.Repositories
{
    public class LogServiceRepository : BaseRepository<LogEntry, LogServiceContext>
    {
        public LogServiceRepository(LogServiceContext context) : base(context)
        {
            RepoTable = context.LogsEntries;
        }

        public override Task Update(LogEntry entity)
        {
           Log.Fatal("TRIED TO ALTER LOGENTRIES!!!");
           return Task.CompletedTask;
        }

        public List<LogEntry> GetLogsByDateAfter(DateTime date)
        {
            return RepoTable.Where(entry => entry.LoggingTime > date).ToList();
        }

        public List<LogEntry> GetLogsByDateBefore(DateTime date)
        {
            return RepoTable.Where(entry => entry.LoggingTime < date).ToList();
        }

        public List<LogEntry> GetLogsByDateBetween(DateTime lower, DateTime upper)
        {
            return RepoTable.Where(entry => entry.LoggingTime < upper && entry.LoggingTime > lower).ToList();
        }

        public List<LogEntry> GetLogsByLogLevel(ELevel level)
        {
            return RepoTable.Where(entry => entry.Level == level).ToList();
        }
    }
}
