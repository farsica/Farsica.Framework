namespace Farsica.Framework.DataAccess.Entities
{
    using Farsica.Framework.DataAnnotation.Schema;

    public interface IEnableable<TClass>
        where TClass : class
    {
        [NotMapped]
        bool Enabled { get; set; }
    }
}
