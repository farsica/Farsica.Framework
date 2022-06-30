namespace Farsica.Framework.DataAccess.Repositories
{
    using Microsoft.EntityFrameworkCore;

    public interface IRepositoryInjection
    {
        IRepositoryInjection SetContext(DbContext context);
    }
}