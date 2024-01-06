namespace Farsica.Framework.DataAccess.Specification.Impl
{
    using System;
    using System.Linq.Expressions;
    using Farsica.Framework.DataAccess.Entities;

    public sealed class DeletedSpecification<TClass>(bool deleted) : SpecificationBase<TClass>
        where TClass : class, IDeletable<TClass>
    {
        private readonly bool deleted = deleted;

        public override Expression<Func<TClass, bool>> Expression() => t => t.Deleted == deleted;
    }
}
