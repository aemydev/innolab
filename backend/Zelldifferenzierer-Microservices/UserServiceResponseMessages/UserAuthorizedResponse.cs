using System;
using System.Collections.Generic;
using System.Text;

namespace UserServiceResponseMessages
{
    public interface UserAuthorizedResponse
    {
        bool IsAuthorized { get; }
    }
}
