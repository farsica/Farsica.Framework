namespace Farsica.Framework.Data
{
    using Farsica.Framework.Data.Enumeration;
    using Farsica.Framework.DataAnnotation;
    using Farsica.Framework.Resources;

    public class DayOfWeek : FlagsEnumeration<DayOfWeek>
    {
        [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(DayOfWeek))]
        public static readonly Flag<DayOfWeek> Sunday = new(0);

        [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(DayOfWeek))]
        public static readonly Flag<DayOfWeek> Monday = new(1);

        [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(DayOfWeek))]
        public static readonly Flag<DayOfWeek> Tuesday = new(2);

        [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(DayOfWeek))]
        public static readonly Flag<DayOfWeek> Wednesday = new(3);

        [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(DayOfWeek))]
        public static readonly Flag<DayOfWeek> Thursday = new(4);

        [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(DayOfWeek))]
        public static readonly Flag<DayOfWeek> Friday = new(5);

        [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(DayOfWeek))]
        public static readonly Flag<DayOfWeek> Saturday = new(6);
    }
}
