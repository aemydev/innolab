using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;

namespace GatewayService.Validators
{
    public static class MultiValidator<T>
    {
        private static readonly List<IValidator<T>> Validators;

        static MultiValidator()
        {
            Validators ??= new List<IValidator<T>>();

            if (Validators.Any()) return;
            foreach (var instance in MultiValidatorBase.Validators.Select(validator => validator.MakeGenericType(typeof(T))).Select(Activator.CreateInstance))
            {
                Validators.Add((IValidator<T>)instance ?? throw new InvalidOperationException());
            }
        }

        public static async Task<bool> IsValid(T message)
        {
            var result = true;
            foreach (var validator in Validators)
            {
                if (!await validator.IsValid(message))
                {
                    result = false;
                }
            }
            return result;
        }
    }
}
