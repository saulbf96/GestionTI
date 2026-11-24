using GestionTI.Application.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionTI.Server.Controllers;

/// <summary>
/// Base de todos los controllers: rutas bajo /api y, como mínimo, la política de lectura.
/// Las acciones que modifican datos agregan su propia política (Operate o Administer).
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = AppPolicies.Read)]
public abstract class ApiControllerBase : ControllerBase;
