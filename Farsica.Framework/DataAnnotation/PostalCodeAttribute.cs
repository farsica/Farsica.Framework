namespace Farsica.Framework.DataAnnotation
{
    using System.Collections.Generic;
    using System.Linq;

    using Farsica.Framework.Core;

    public sealed class PostalCodeAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (string.IsNullOrEmpty(value?.ToString()))
            {
                return true;
            }

            if (value is IEnumerable<string> lst)
            {
                return lst.All(t => string.IsNullOrEmpty(t) || Globals.ValidatePostalCode(t));
            }

            return Globals.ValidatePostalCode(value.ToString());
        }
    }
}
