using GestionTI.Application.Security;
using Microsoft.Extensions.Logging;

namespace GestionTI.Infrastructure.Security;

/// <summary>
/// Implementación temporal: escribe el acceso denegado en el log de la aplicación.
/// Se reemplaza por el registro en la tabla Movimientos cuando exista.
/// </summary>
public sealed partial class LoggingAccessDeniedLog(ILogger<LoggingAccessDeniedLog> logger) : IAccessDeniedLog
{
    public Task RecordAsync(AccessDeniedEntry entry, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(entry);

        LogAccessDenied(logger, entry.UserName, entry.Method, entry.Resource, entry.OccurredAt);
        return Task.CompletedTask;
    }

    [LoggerMessage(
        EventId = 4030,
        Level = LogLevel.Warning,
        Message = "Acceso denegado: {UserName} intentó {Method} {Resource} el {OccurredAt:o}")]
    private static partial void LogAccessDenied(
        ILogger logger, string userName, string method, string resource, DateTimeOffset occurredAt);
}
