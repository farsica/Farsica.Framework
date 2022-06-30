namespace Farsica.Framework.DataAnnotation.Schema
{
    using System;

    public sealed class DatabaseGeneratedAttribute : System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedAttribute
    {
        public DatabaseGeneratedAttribute(DatabaseGeneratedOption databaseGeneratedOption, string sequenceName = null)
            : base(Convert(databaseGeneratedOption))
        {
            SequenceName = sequenceName;
            DatabaseGeneratedOption = databaseGeneratedOption;
        }

        public string SequenceName { get; }

        public new DatabaseGeneratedOption DatabaseGeneratedOption { get; }

        private static System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption Convert(DatabaseGeneratedOption databaseGeneratedOption)
        {
            switch (databaseGeneratedOption)
            {
                case Schema.DatabaseGeneratedOption.None:
                    return System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None;
                case Schema.DatabaseGeneratedOption.Identity:
                    return System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity;
                case Schema.DatabaseGeneratedOption.Computed:
                    return System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Computed;
                default:
                    throw new ArgumentException(databaseGeneratedOption.ToString());
            }
        }
    }
}
