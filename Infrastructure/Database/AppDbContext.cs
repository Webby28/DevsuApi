using WebApi.Infrastructure.Database.Helpers;

namespace WebApi.Infrastructure.Database;

public interface IAppDb
{
    IApplicationDbContext OracleDbContext { get; }
}

public class AppAppDb : IAppDb
{
    public IApplicationDbContext OracleDbContext { get; }

    public AppAppDb(Func<TipoConexion, IApplicationDbContext> dbContextResolverDelegate)
    {
        OracleDbContext = dbContextResolverDelegate(TipoConexion.Oracle);
    }
}