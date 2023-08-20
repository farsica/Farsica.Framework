namespace Farsica.Framework.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Farsica.Framework.Data;

    public class ConnectionString
    {
        public ConnectionString(DbProviderType providerType, string? host, string? dbIdentifier, string? user, string? password, ushort? port = null, Dictionary<string, string>? parameters = null)
        {
            ProviderType = providerType;
            Host = host;
            Port = port;
            DbIdentifier = dbIdentifier;
            User = user;
            Password = password;
            Parameters = parameters;
        }

        public DbProviderType ProviderType { get; set; }

        public string? Host { get; set; }

        public ushort? Port { get; set; }

        public string? DbIdentifier { get; set; }

        public string? User { get; set; }

        public string? Password { get; set; }

        public Dictionary<string, string> Parameters { get; set; }

        public override string? ToString()
        {
            var sb = new StringBuilder();
            switch (ProviderType)
            {
                case DbProviderType.SqlServer:
                    sb.Append($"Server={Host};Database={DbIdentifier};User Id={User};Password={Password};");

                    if (Port.HasValue)
                    {
                        sb.Append($"Port={Port.Value};");
                    }

                    break;
                case DbProviderType.DevartOracle:
                    if (!Port.HasValue)
                    {
                        Port = 1521;
                    }

                    sb.Append($"User Id={User};Password={Password};Direct=true;Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=tcp)(HOST={Host})(PORT={Port}))(CONNECT_DATA=(SID={DbIdentifier})))");

                    break;
                default:
                    throw new NotImplementedException(ProviderType.ToString());
            }

            if (Parameters?.Any() is true)
            {
                sb.AppendJoin(";", Parameters.Keys.Select(t => $"{t}={Parameters[t]}"));
            }

            return sb.ToString();
        }
    }
}
