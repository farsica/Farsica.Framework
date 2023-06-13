namespace Farsica.Framework.DataAccess.Entities
{
    using Farsica.Framework.DataAnnotation.Schema;

    public interface IDeleteable<TClass>
        where TClass : class
    {
        [NotMapped]
        bool Deleted { get; set; }
    }
}
