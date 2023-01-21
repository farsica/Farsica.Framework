namespace Farsica.Framework.Data
{
    using Farsica.Framework.DataAnnotation;
    using static Farsica.Framework.Core.Constants;

    public sealed class SortFilter
    {
        public SortType SortType { get; set; }

        [Required]
        public string? Column { get; set; }
    }
}
