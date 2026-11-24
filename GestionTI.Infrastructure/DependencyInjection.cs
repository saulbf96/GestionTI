using GestionTI.Application.Security;
using GestionTI.Infrastructure.Persistence;
using GestionTI.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GestionTI.Infrastructure;

public static class DependencyInjection
{
    public const string ConnectionStringName = "GestionTI";

    /// <summary>
    /// Registra base de datos, Active Directory y demás servicios externos.
    /// </summary>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        // La cadena de conexión se lee hasta que se usa la base de datos,
        // así la API puede arrancar aunque todavía no esté configurada.
        services.AddDbContext<GestionTIDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString(ConnectionStringName);
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException(
                    $"Falta la cadena de conexión 'ConnectionStrings:{ConnectionStringName}'. " +
                    "En desarrollo configúrala con user-secrets; en servidores, con variables de entorno.");
            }

            options.UseSqlServer(connectionString);
        });

        services.AddScoped<IAccessDeniedLog, LoggingAccessDeniedLog>();

        return services;
    }
}
