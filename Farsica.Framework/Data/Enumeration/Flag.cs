namespace Farsica.Framework.Data.Enumeration
{
    using System.Collections;

    public class Flag : Flag<object>
    {
        public Flag(int index, int? length = null)
            : base(index, length)
        {
        }

        public Flag(BitArray new_value)
            : base(new_value)
        {
        }

        public Flag(byte[] new_value)
            : base(new_value)
        {
        }
    }
}
