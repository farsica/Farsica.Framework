namespace Farsica.Framework.DataAnnotation
{
    using Farsica.Framework.Core;

    public sealed class NationalCodeAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (string.IsNullOrWhiteSpace(value?.ToString()))
            {
                return true;
            }

            return Globals.ValidateNationalCode(value.ToString());
        }
    }
}