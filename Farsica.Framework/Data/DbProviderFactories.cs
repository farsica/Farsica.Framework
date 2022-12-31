namespace Farsica.Framework.Data
{
    using Farsica.Framework.Core;

    public class DbProviderFactories
    {
        private static DbProviderFactory factory;

        public static DbProviderFactory GetFactory
        {
            get
            {
                if (factory is not null)
                {
                    return factory;
                }

                switch (Globals.ProviderType)
                {
                    case ProviderType.SqlServer:
                        factory = new SqlServerProvider();
                        break;
                    case ProviderType.DevartOracle:
                        factory = new DevartOracleProvider();
                        break;
                    default:
                        throw new System.NotSupportedException(Globals.ProviderType.ToString());
                }

                return factory;
            }
        }
    }
}
