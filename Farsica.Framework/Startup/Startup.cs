namespace Farsica.Framework.Startup
{
    public abstract class Startup : Startup<Startup, Startup>
    {
        protected Startup(StartupOption startupOption)
            : base(startupOption)
        {
        }
    }
}
