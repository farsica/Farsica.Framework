namespace Farsica.Framework.DataAnnotation
{
    using System.Collections.Generic;
    using System.Linq;

    public sealed class IpAddressAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (string.IsNullOrEmpty(value?.ToString()))
            {
                return true;
            }

            var lst = value as List<string>;
            return lst?.All(Validate) ?? Validate(value.ToString());
        }

        private static bool Validate(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return true;
            }

            var splitValues = value.Split('.');
            if (splitValues.Length != 4)
            {
                return false;
            }

            return splitValues.All(r => byte.TryParse(r, out byte tmp));
        }
    }
}