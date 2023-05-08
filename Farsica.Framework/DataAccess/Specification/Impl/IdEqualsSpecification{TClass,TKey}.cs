﻿namespace Farsica.Framework.DataAccess.Specification.Impl
{
    using System;
    using System.Linq.Expressions;
    using Farsica.Framework.DataAccess.Entities;

    public sealed class IdEqualsSpecification<TClass, TKey> : SpecificationBase<TClass>
        where TClass : class, IIdentifiable<TClass, TKey>
        where TKey : IEquatable<TKey>
    {
        private readonly TKey id;

        public IdEqualsSpecification(TKey id)
        {
            this.id = id;
        }

        public override Expression<Func<TClass, bool>> Expression() => t => t.Id.Equals(id);
    }
}
