namespace Farsica.Framework.Core.Constant
{
    using Farsica.Framework.Data.Enumeration;
    using Farsica.Framework.DataAnnotation;
    using Farsica.Framework.Resources;

    public sealed class GenderType(string name, byte value) : Enumeration<byte>(name, value)
    {
        [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(GenderType))]
        public static readonly GenderType Male = new(nameof(Male), 0);

        [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(GenderType))]
        public static readonly GenderType Female = new(nameof(Female), 1);
    }
}
