namespace Farsica.Framework.Converter
{
    public interface IJsonConverter
    {
        bool IgnoreOnExport { get; }

        string? Convert(object value);
    }
}
