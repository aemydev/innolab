using System;
using System.Collections.Generic;
using System.Text;

namespace LogServiceRequestMessages
{
    public interface GetLogsByDateBefore
    {
        string AdminId { get; }
        DateTime Date { get; }
    }
}
