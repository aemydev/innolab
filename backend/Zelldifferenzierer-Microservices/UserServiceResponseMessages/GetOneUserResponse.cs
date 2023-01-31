using System;
using System.Collections.Generic;
using System.Text;
using UserServiceModels;

namespace UserServiceResponseMessages
{
    public interface GetOneUserResponse
    {
        ApplicationUser User { get; }
    }
}
