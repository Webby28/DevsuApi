using Microsoft.Extensions.Configuration;

namespace WebApi.Infrastructure.Database.Helpers;

public sealed class ConnectionStringFactory : IConnectionStringFactory
{
    private readonly IConfiguration _configuration;
    private readonly Func<TipoConexion, string> _connectionStringResolver;

    public ConnectionStringFactory(IConfiguration configuration)
    {
        _configuration = configuration;
        _connectionStringResolver = ConnectionStringResolver();
    }

    public string GetConnectionString(TipoConexion tipoConexion) => _connectionStringResolver(tipoConexion);

    private Func<TipoConexion, string> ConnectionStringResolver()
    {
        return key =>
        {
            return key switch
            {
                TipoConexion.Oracle => _configuration.GetConnectionString(key.ToString()),
                _ => throw new ArgumentOutOfRangeException(nameof(key), key, "Cadena de conexion no implementada")
            };
        };
    }
}