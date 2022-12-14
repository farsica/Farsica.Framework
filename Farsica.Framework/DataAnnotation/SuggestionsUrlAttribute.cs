namespace Farsica.Framework.DataAnnotation
{
    using System;

    [AttributeUsage(AttributeTargets.Property)]
    public sealed class SuggestionsUrlAttribute : Attribute
    {
        public SuggestionsUrlAttribute(string url)
        {
            Url = url;
        }

        public string? Url { get; }
    }
}
