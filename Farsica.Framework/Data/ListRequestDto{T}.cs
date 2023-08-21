namespace Farsica.Framework.Data
{
    using Farsica.Framework.DataAccess.Specification;

    public class ListRequestDto<T>
    {
        public ISpecification<T>? Specification { get; set; }

        public PagingDto? PagingDto { get; set; }
    }
}
