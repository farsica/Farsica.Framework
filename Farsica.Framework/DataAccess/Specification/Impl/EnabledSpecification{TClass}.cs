namespace Farsica.Framework.DataAccess.Specification.Impl
{
    using System;
    using System.Linq.Expressions;
    using Farsica.Framework.DataAccess.Entities;

    public sealed class EnabledSpecification<TClass> : SpecificationBase<TClass>
        where TClass : class, IEnablable<TClass>
    {
        private readonly bool enabled;

        public EnabledSpecification(bool enabled)
        {
            this.enabled = enabled;
        }

        public override Expression<Func<TClass, bool>> Expression() => t => t.Enabled == enabled;
    }
}
