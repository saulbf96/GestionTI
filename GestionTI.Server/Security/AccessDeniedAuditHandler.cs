using GestionTI.Application.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;

namespace GestionTI.Server.Security;

/// <summary>
/// Registra en la bitácora cada acceso denegado (403) y después responde como siempre.
/// </summary>
public sealed class AccessDeniedAuditHandler(TimeProvider timeProvider) : IAuthorizationMiddlewareResultHandler
{
    private readonly AuthorizationMiddlewareResultHandler _defaultHandler = new();

    public async Task HandleAsync(
        RequestDelegate next, HttpContext context, AuthorizationPolicy policy, PolicyAuthorizationResult authorizeResult)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(authorizeResult);

        if (authorizeResult.Forbidden)
        {
            var log = context.RequestServices.GetRequiredService<IAccessDeniedLog>();
            await log.RecordAsync(
                new AccessDeniedEntry(
                    context.User.Identity?.Name ?? "(desconocido)",
                    context.Request.Method,
                    context.Request.Path.Value ?? string.Empty,
                    timeProvider.GetUtcNow()),
                context.RequestAborted);
        }

        await _defaultHandler.HandleAsync(next, context, policy, authorizeResult);
    }
}
