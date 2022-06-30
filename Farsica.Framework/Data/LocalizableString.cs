namespace Farsica.Framework.Data
{
    using System.Collections.Generic;

    public class LocalizableString
    {
        public string Value { get; set; }

        public IReadOnlyList<LocalizedValue> LocalizedValues { get; set; }

        public override string ToString()
        {
            return Value;
        }

        public struct LocalizedValue
        {
            public short Id { get; set; }

            public string Code { get; set; }

            public string Value { get; set; }
        }
    }
}
