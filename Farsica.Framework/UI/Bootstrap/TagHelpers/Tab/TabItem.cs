namespace Farsica.Framework.UI.Bootstrap.TagHelpers.Tab
{
    public class TabItem(string? header, string? content, bool active, string? id, string? parentId, bool isDropdown)
    {
        public string? Header { get; set; } = header;

        public string? Content { get; set; } = content;

        public bool Active { get; set; } = active;

        public bool IsDropdown { get; set; } = isDropdown;

        public string? Id { get; set; } = id;

        public string? ParentId { get; set; } = parentId;
    }
}
