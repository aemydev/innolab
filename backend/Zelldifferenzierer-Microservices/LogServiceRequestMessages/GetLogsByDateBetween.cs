using System;
using System.Collections.Generic;
using System.Text;

namespace LogServiceRequestMessages
{
    public interface GetLogsByDateBetween
    {
        string AdminId { get; }
        DateTime DateAfter { get; }
        DateTime DateBefore { get; }
    }
}
