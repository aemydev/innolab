using LogServiceModels;

namespace LogServiceRequestMessages
{
    public interface LogRequest
    {
         string LogMessage { get; }
         string Sender { get; }
         ELevel Level { get; }
    }
}
