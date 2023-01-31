namespace Farsica.Framework.Data
{
    using Farsica.Framework.DataAnnotation;

    public sealed class PageFilter
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int CurrentPage { get; set; } = 1;

        [Required]
        [Range(1, int.MaxValue)]
        public int PageSize { get; set; } = 1;

        public ExportType? ExportType { get; set; }

        public bool ReturnTotalRecordsCount { get; set; }
    }
}
