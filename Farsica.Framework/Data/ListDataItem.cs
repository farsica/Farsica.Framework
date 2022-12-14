namespace Farsica.Framework.Data
{
    using System.Text.Json.Serialization;

    public class ListDataItem<T>
    {
        [JsonPropertyName("text")]
        public string? Name { get; set; }

        [JsonPropertyName("id")]
        public T Value { get; set; }
    }
}
