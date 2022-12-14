namespace Farsica.Framework.Data
{
    using Farsica.Framework.DataAnnotation;

    public record SortFilter
    {
        public bool Descending { get; set; }

        [Required]
        public string? Column { get; set; }
    }
}
