using GestionTI.Application.Security;
using GestionTI.Server.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace GestionTI.Server.Controllers;

/// <summary>
/// Datos de la sesión actual. Angular lo usa para mostrar el nombre y ocultar
/// menús y botones según el rol (la seguridad real se valida en cada endpoint).
/// </summary>
public sealed class SessionController : ApiControllerBase
{
    [HttpGet]
    public ActionResult<SessionResponse> Get() =>
        new SessionResponse(
            User.Identity?.Name ?? string.Empty,
            AppRoles.All.Where(User.IsInRole).ToList());
}
