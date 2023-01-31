using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GatewayService.Validators
{
    public interface IValidator<in T>
    {
        Task<bool> IsValid(T message);
    }
}
