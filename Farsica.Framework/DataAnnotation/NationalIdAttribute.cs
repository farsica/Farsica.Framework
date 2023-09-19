namespace Farsica.Framework.DataAnnotation
{
    using Farsica.Framework.Core;

    public sealed class NationalIdAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (string.IsNullOrEmpty(value?.ToString()))
            {
                return true;
            }

            return Globals.ValidateNationalId(value.ToString()!);
        }
    }
}
