namespace Farsica.Framework.Mapping
{
    using System;

    public class AdaptTwoWaysAttribute : Mapster.AdaptTwoWaysAttribute
    {
        public AdaptTwoWaysAttribute(Type type)
            : base(type)
        {
        }

        public AdaptTwoWaysAttribute(string name)
            : base(name)
        {
        }
    }
}
