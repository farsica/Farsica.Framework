namespace Farsica.Framework.Data
{
    using System.Text.Json.Serialization;

    public readonly struct Paging(bool moreRecords)
    {
        [JsonPropertyName("more")]
        public bool MoreRecords { get; } = moreRecords;
    }
}
