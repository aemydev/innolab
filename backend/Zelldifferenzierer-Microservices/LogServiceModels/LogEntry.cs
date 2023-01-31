using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Tracing;

namespace LogServiceModels
{
    public enum ELevel { Information, Warning, Error, Fatal }
    /// <summary>
    /// DB Model for a log entry 
    /// </summary>
    public class LogEntry
    {
        [Key]
        public int Id { get; private set; }

        [Required]
        public DateTime LoggingTime { get; private set; }

        [Required]
        public string Sender { get; private set; }

        [Required]
        public ELevel Level { get; private set; }

        [Required]
        public string LogMessage { get; private set; }
        
        [Required]
        public DateTime? SentTime { get; private set; }


        public LogEntry(string logMessage, string sender, DateTime? sentTime, ELevel level)
        {
            LoggingTime = DateTime.Now;
            LogMessage = logMessage;
            Sender = sender;
            SentTime = sentTime;
            Level = level;
        }

        public LogEntry () { }

        public override string ToString()
        {
            return $"{Level} received from {Sender} at {LoggingTime} \n Message: {LogMessage} TimeStamp: {SentTime} ";
        }
    }
}
