using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GatewayService.Validators
{
    public static class MultiValidatorBase
    {
        public static List<Type> Validators { get; } = new List<Type>();
    }
}
