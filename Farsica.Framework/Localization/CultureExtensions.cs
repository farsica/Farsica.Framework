namespace Farsica.Framework.Localization
{
    using System.Collections.Generic;

    public static class CultureExtensions
    {
        public static IEnumerable<string> GetAtomicValues()
        {
            yield return nameof(Culture.Fa);
            yield return nameof(Culture.En);
        }
    }
}
