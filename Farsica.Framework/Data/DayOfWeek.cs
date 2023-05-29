namespace Farsica.Framework.Data
{
    using System.Collections;
    using Farsica.Framework.Data.Enumeration;
    using Farsica.Framework.DataAnnotation;
    using Farsica.Framework.Resources;

    public class DayOfWeek : FlagsEnumeration<DayOfWeek>
    {
        [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(DayOfWeek))]
        public static readonly DayOfWeek Sunday = new(1);

        [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(DayOfWeek))]
        public static readonly DayOfWeek Monday = new(2);

        [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(DayOfWeek))]
        public static readonly DayOfWeek Tuesday = new(3);

        [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(DayOfWeek))]
        public static readonly DayOfWeek Wednesday = new(4);

        [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(DayOfWeek))]
        public static readonly DayOfWeek Thursday = new(5);

        [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(DayOfWeek))]
        public static readonly DayOfWeek Friday = new(6);

        [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(DayOfWeek))]
        public static readonly DayOfWeek Saturday = new(7);

        public DayOfWeek()
        {
        }

        public DayOfWeek(BitArray value)
            : base(value)
        {
        }

        public DayOfWeek(byte[] value)
            : base(value)
        {
        }

        public DayOfWeek(int index, int? length = null)
            : base(index, length)
        {
        }
    }
}
