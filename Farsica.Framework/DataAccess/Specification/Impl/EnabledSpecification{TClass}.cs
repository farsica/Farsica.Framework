namespace Farsica.Framework.DataAccess.Specification.Impl
{
    using System;
    using System.Linq.Expressions;
    using Farsica.Framework.DataAccess.Entities;

    public sealed class EnabledSpecification<TClass>(bool enabled) : SpecificationBase<TClass>
        where TClass : class, IEnablable<TClass>
    {
        private readonly bool enabled = enabled;

        public override Expression<Func<TClass, bool>> Expression() => t => t.Enabled == enabled;
    }
}
