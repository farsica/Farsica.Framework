namespace Farsica.Framework.DataAccess.Concurrency
{
    using Farsica.Framework.Data;
    using Farsica.Framework.DataAnnotation.Schema;

    public interface IRowVersion
    {
        [System.ComponentModel.DataAnnotations.ConcurrencyCheck]
        [Column(nameof(RowVersion), DataType.Short)]
        long RowVersion { get; set; }
    }
}
