namespace Farsica.Framework.DataAnnotation.Schema
{
    using System;

    public sealed class DatabaseGeneratedAttribute(DatabaseGeneratedOption databaseGeneratedOption, string? sequenceName = null) : System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedAttribute(Convert(databaseGeneratedOption))
    {
        public string? SequenceName { get; } = sequenceName;

        public new DatabaseGeneratedOption DatabaseGeneratedOption { get; } = databaseGeneratedOption;

        private static System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption Convert(DatabaseGeneratedOption databaseGeneratedOption)
        {
            switch (databaseGeneratedOption)
            {
                case DatabaseGeneratedOption.None:
                    return System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None;
                case DatabaseGeneratedOption.Identity:
                    return System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity;
                case DatabaseGeneratedOption.Computed:
                    return System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Computed;
                default:
                    throw new ArgumentException(databaseGeneratedOption.ToString());
            }
        }
    }
}
