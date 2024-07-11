namespace Farsica.Framework.Data
{
    using Farsica.Framework.Core;

    public class DbProviderFactories
    {
        private static DbProviderFactory? factory;

        public static DbProviderFactory GetFactory
        {
            get
            {
                if (factory is not null)
                {
                    return factory;
                }

                factory = Globals.ProviderType switch
                {
                    DbProviderType.SqlServer => new SqlServerProvider(),
                    DbProviderType.DevartOracle => new DevartOracleProvider(),
                    DbProviderType.MySql => new MySqlProvider(),
                    _ => throw new System.NotSupportedException(Globals.ProviderType.ToString()),
                };
                return factory;
            }
        }
    }
}
