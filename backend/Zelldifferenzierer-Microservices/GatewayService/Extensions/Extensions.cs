using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GatewayService.Validators;
using MassTransit;

namespace GatewayService.Extensions
{
    public static class Extensions
    {
        public static Task<bool> IsValid<T>(this T obj) => MultiValidator<T>.IsValid(obj);
    }
}
