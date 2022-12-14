namespace Farsica.Framework.Data
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    public class ListDataSource<T>
    {
        [JsonPropertyName("results")]
        public IList<ListDataItem<T>>? Data { get; set; }

        [JsonPropertyName("pagination")]
        public Paging Pagination => new(Data?.Count == 10);
    }
}
