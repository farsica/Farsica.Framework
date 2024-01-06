namespace Farsica.Framework.Core.Constant
{
    using Farsica.Framework.Data.Enumeration;
    using Farsica.Framework.DataAnnotation;
    using Farsica.Framework.Resources;

    public sealed class UserType(string name, byte value) : Enumeration<byte>(name, value)
    {
        [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(UserType))]
        public static readonly UserType Real = new(nameof(Real), 0);

        [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(UserType))]
        public static readonly UserType Legal = new(nameof(Legal), 1);
    }
}
