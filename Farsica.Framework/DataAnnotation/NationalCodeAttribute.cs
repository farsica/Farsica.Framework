namespace Farsica.Framework.DataAnnotation
{
    using System.Collections.Generic;
    using System.Linq;
    using Farsica.Framework.Core;

    public sealed class NationalCodeAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (string.IsNullOrEmpty(value?.ToString()))
            {
                return true;
            }

            if (value is IEnumerable<string> lst)
            {
                return lst.All(t => string.IsNullOrEmpty(t) || Globals.ValidateNationalCode(t));
            }

            return Globals.ValidateNationalCode(value.ToString()!);
        }
    }
}
