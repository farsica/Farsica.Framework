namespace Farsica.Framework.Core
{
    using System.Collections.Generic;

    public class Tenant
    {
        public string? Name { get; set; }

        public string? Code { get; set; }

        public string? Schema { get; set; }

        public string? ArchiveSchema { get; set; }

        public IReadOnlyList<string?> Domains { get; set; }
    }
}
