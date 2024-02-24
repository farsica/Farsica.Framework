namespace Farsica.Framework.DataAccess.Audit
{
    using System.Threading.Tasks;
    using Farsica.Framework.Data;
    using Farsica.Framework.DataAccess.Specification;
    using Farsica.Framework.DataAnnotation;

    [Injectable]
    public interface IAuditService
    {
        Task<ResultData<ListDataSource<AuditListDto>>> GetAuditsAsync(ListRequestDto<Audit>? requestDto = null);

        Task<ResultData<AuditDto>> Get(ISpecification<Audit> specification);
    }
}
