namespace Farsica.Framework.DataAccess.Context
{
    using Microsoft.EntityFrameworkCore.Infrastructure;

    public static class DbContextOptionsBuilderExtensions
    {
        public static void MapMigrationsHistoryTable<TBuilder, TExtension>(this RelationalDbContextOptionsBuilder<TBuilder, TExtension> builder, string? defaultSchema, string migrationsHistoryTableName = "_MigrationsHistory")
            where TBuilder : RelationalDbContextOptionsBuilder<TBuilder, TExtension>
            where TExtension : RelationalOptionsExtension, new()
        {
            builder.MigrationsHistoryTable(Data.DbProviderFactories.GetFactory.GetObjectName(migrationsHistoryTableName, pluralize: false), defaultSchema);
        }
    }
}
