namespace Farsica.Framework.Core.Constant
{
    using Farsica.Framework.Data.Enumeration;
    using Farsica.Framework.DataAnnotation;
    using Farsica.Framework.Resources;

    public sealed class DegreeType(string name, byte value) : Enumeration<DegreeType, byte>(name, value)
    {
        [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(DegreeType))]
        public static readonly DegreeType Diploma = new(nameof(Diploma), 0);

        [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(DegreeType))]
        public static readonly DegreeType Associate = new(nameof(Associate), 1);

        [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(DegreeType))]
        public static readonly DegreeType Bachelor = new(nameof(Bachelor), 2);

        [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(DegreeType))]
        public static readonly DegreeType Master = new(nameof(Master), 3);

        [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(DegreeType))]
        public static readonly DegreeType Phd = new(nameof(Phd), 4);

        [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(DegreeType))]
        public static readonly DegreeType PostDoc = new(nameof(PostDoc), 5);

        [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(DegreeType))]
        public static readonly DegreeType Other = new(nameof(Other), 6);
    }
}
