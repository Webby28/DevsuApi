namespace WebApi.Infrastructure.Database.Helpers;

public interface IConnectionStringFactory
{
    string GetConnectionString(TipoConexion tipoConexion);
}