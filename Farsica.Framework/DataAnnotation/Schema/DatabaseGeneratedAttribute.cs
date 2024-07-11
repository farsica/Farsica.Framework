namespace Farsica.Framework.DataAnnotation.Schema
{
    using System;

    public sealed class DatabaseGeneratedAttribute(DatabaseGeneratedOption databaseGeneratedOption, string? sequenceName = null) : System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedAttribute(Convert(databaseGeneratedOption))
    {
        public string? SequenceName { get; } = sequenceName;

        public new DatabaseGeneratedOption DatabaseGeneratedOption { get; } = databaseGeneratedOption;

        private static System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption Convert(DatabaseGeneratedOption databaseGeneratedOption)
        {
            return databaseGeneratedOption switch
            {
                DatabaseGeneratedOption.None => System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None,
                DatabaseGeneratedOption.Identity => System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity,
                DatabaseGeneratedOption.Computed => System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Computed,
                _ => throw new ArgumentException(databaseGeneratedOption.ToString()),
            };
        }
    }
}
