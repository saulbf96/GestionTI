namespace GestionTI.Application.Security;

/// <summary>
/// Registra en la bitácora los intentos de acceso sin permiso.
/// </summary>
public interface IAccessDeniedLog
{
    Task RecordAsync(AccessDeniedEntry entry, CancellationToken cancellationToken);
}

/// <param name="UserName">Cuenta de red, por ejemplo UNOCONTACT\jperez.</param>
/// <param name="Method">Método HTTP.</param>
/// <param name="Resource">Ruta solicitada.</param>
/// <param name="OccurredAt">Fecha y hora (UTC).</param>
public sealed record AccessDeniedEntry(string UserName, string Method, string Resource, DateTimeOffset OccurredAt);
