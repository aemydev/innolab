using System;
using System.Collections.Generic;
using System.Text;

namespace UserServiceRequestMessages
{
    public interface UserAuthorizedRequest
    {
        string UserId { get; }
        int FolderId { get; }
    }
}
