using Microsoft.Extensions.DependencyInjection;

namespace GestionTI.Application;

public static class DependencyInjection
{
    /// <summary>
    /// Registra los servicios de la capa Application (casos de uso y validaciones).
    /// </summary>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        // Los casos de uso se registran aquí conforme se agreguen.
        return services;
    }
}
