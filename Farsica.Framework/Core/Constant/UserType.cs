namespace Farsica.Framework.Core.Constant
{
    using Farsica.Framework.Data.Enumeration;
    using Farsica.Framework.DataAnnotation;
    using Farsica.Framework.Resources;

    public sealed class UserType : Enumeration<byte>
    {
        [Display(ResourceType = typeof(GlobalResource))]
        public static readonly UserType Real = new(nameof(Real), 0);

        [Display(ResourceType = typeof(GlobalResource))]
        public static readonly UserType Legal = new(nameof(Legal), 1);

        public UserType(string name, byte value)
            : base(name, value)
        {
        }
    }
}
