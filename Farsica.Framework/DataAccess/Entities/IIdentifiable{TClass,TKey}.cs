namespace Farsica.Framework.DataAccess.Entities
{
    using System;
    using Farsica.Framework.DataAnnotation.Schema;

    public interface IIdentifiable<TClass, TKey>
        where TClass : class
        where TKey : IEquatable<TKey>
    {
        [NotMapped]
        TKey Id { get; set; }
    }
}
