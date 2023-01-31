namespace Farsica.Framework.Data
{
    using Farsica.Framework.DataAnnotation;
    using Farsica.Framework.Resources;

    public class ExportType : Enumeration.Enumeration<byte>
    {
        [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(ExportType))]
        public static readonly ExportType Excel = new(nameof(Excel), 0);

        [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(ExportType))]
        public static readonly ExportType Pdf = new(nameof(Pdf), 1);

        [Display(ResourceType = typeof(GlobalResource), EnumType = typeof(ExportType))]
        public static readonly ExportType Csv = new(nameof(Csv), 2);

        public ExportType()
        {
        }

        public ExportType(string name, byte value)
            : base(name, value)
        {
        }
    }
}
