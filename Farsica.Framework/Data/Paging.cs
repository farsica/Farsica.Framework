namespace Farsica.Framework.Data
{
    using System.Text.Json.Serialization;

    public struct Paging
    {
        public Paging(bool moreRecords)
        {
            MoreRecords = moreRecords;
        }

        [JsonPropertyName("more")]
        public bool MoreRecords { get; }
    }
}
