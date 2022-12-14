namespace Farsica.Framework.Data
{
    using static Farsica.Framework.Core.Constants;

    public class PagingDto
    {
        public int CurrentPage { get; set; }

        public int PageSize { get; set; }

        public SortFilter? SortFilter { get; set; }

        public SearchFilter? SearchFilter { get; set; }

        public bool Export { get; set; }

        public ExportType ExportType { get; set; }
    }
}
