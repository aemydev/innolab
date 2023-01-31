using System;
using System.Collections.Generic;
using System.Text;

namespace UserServiceRequestMessages
{
    public interface DeleteUserRequest
    {
        string UserId { get; }
    }
}
