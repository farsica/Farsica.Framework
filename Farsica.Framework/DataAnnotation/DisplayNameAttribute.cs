namespace Farsica.Framework.DataAnnotation
{
    public sealed class DisplayNameAttribute : System.ComponentModel.DisplayNameAttribute
    {
        public DisplayNameAttribute()
            : base()
        {
        }

        public DisplayNameAttribute(string? displayName)
            : base(displayName ?? string.Empty)
        {
        }
    }
}
