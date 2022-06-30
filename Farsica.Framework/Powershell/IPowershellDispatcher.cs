namespace Farsica.Framework.Powershell
{
    using System.Threading.Tasks;

    public interface IPowershellDispatcher
    {
        Task Dispatch(PowershellContext context);
    }
}
