namespace Farsica.Framework.Data
{
    using Farsica.Framework.DataAnnotation;

    public sealed class SearchFilter
    {
        [Required]
        public string? Phrase { get; set; }

        [Required]
        public string? Column { get; set; }
    }
}
