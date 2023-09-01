namespace Farsica.Framework.Core.Constant
{
    using Farsica.Framework.Data.Enumeration;
    using Farsica.Framework.DataAnnotation;
    using Farsica.Framework.Resources;

    public sealed class GenderType : Enumeration<byte>
    {
        [Display(ResourceType = typeof(GlobalResource))]
        public static readonly GenderType Male = new(nameof(Male), 0);

        [Display(ResourceType = typeof(GlobalResource))]
        public static readonly GenderType Female = new(nameof(Female), 1);

        public GenderType(string name, byte value)
            : base(name, value)
        {
        }
    }
}
