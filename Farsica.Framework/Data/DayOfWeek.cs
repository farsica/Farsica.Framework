namespace Farsica.Framework.Data
{
    using System.Linq;
    using Farsica.Framework.DataAnnotation;
    using Farsica.Framework.Resources;

    public class DayOfWeek : Enumeration.FlagsEnumeration<DayOfWeek, byte>
    {
        [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(DayOfWeek))]
        public static readonly DayOfWeek Sunday = new(nameof(Sunday), 1);

        [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(DayOfWeek))]
        public static readonly DayOfWeek Monday = new(nameof(Monday), 2);

        [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(DayOfWeek))]
        public static readonly DayOfWeek Tuesday = new(nameof(Tuesday), 4);

        [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(DayOfWeek))]
        public static readonly DayOfWeek Wednesday = new(nameof(Wednesday), 8);

        [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(DayOfWeek))]
        public static readonly DayOfWeek Thursday = new(nameof(Thursday), 16);

        [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(DayOfWeek))]
        public static readonly DayOfWeek Friday = new(nameof(Friday), 32);

        [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(DayOfWeek))]
        public static readonly DayOfWeek Saturday = new(nameof(Saturday), 64);

        public DayOfWeek()
        {
        }

        public DayOfWeek(string name, byte value)
            : base(name, value)
        {
        }

        private DayOfWeek(DayOfWeek a, DayOfWeek b)
        {
            foreach (var t in a.Types.Union(b.Types))
            {
                Types[t.Key] = t.Value;
            }

            Name = string.Join(", ", Types.Select(t => t.Key));
            Value = (byte)Types.Values.Sum(t => t);
        }

        protected override DayOfWeek Or(DayOfWeek other)
        {
            return new DayOfWeek(this, other);
        }
    }
}
