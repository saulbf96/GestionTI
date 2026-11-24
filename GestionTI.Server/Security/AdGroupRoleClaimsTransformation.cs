using System.ComponentModel;
using System.Security.Claims;
using System.Security.Principal;
using GestionTI.Application.Security;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace GestionTI.Server.Security;

/// <summary>
/// Convierte la pertenencia a grupos de Active Directory en roles de la aplicación
/// (Admin, Technician, ReadOnly) según la configuración "Security:Groups".
/// </summary>
public sealed partial class AdGroupRoleClaimsTransformation(
    IOptionsMonitor<SecurityOptions> options,
    ILogger<AdGroupRoleClaimsTransformation> logger) : IClaimsTransformation
{
    public const string IdentityType = "GestionTI.Roles";

    public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        ArgumentNullException.ThrowIfNull(principal);

        // Sin sesión, o ya transformado en esta petición: no se toca.
        if (principal.Identity?.IsAuthenticated != true ||
            principal.Identities.Any(i => i.AuthenticationType == IdentityType))
        {
            return Task.FromResult(principal);
        }

        var groups = options.CurrentValue;
        var roles = new List<string>();
        if (IsMemberOfAny(principal, groups.Admin))
        {
            roles.Add(AppRoles.Admin);
        }

        if (IsMemberOfAny(principal, groups.Technician))
        {
            roles.Add(AppRoles.Technician);
        }

        if (IsMemberOfAny(principal, groups.ReadOnly))
        {
            roles.Add(AppRoles.ReadOnly);
        }

        var rolesIdentity = new ClaimsIdentity(IdentityType, ClaimTypes.Name, ClaimTypes.Role);
        rolesIdentity.AddClaims(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var transformed = principal.Clone();
        transformed.AddIdentity(rolesIdentity);
        return Task.FromResult(transformed);
    }

    private bool IsMemberOfAny(ClaimsPrincipal principal, IEnumerable<string> groups) =>
        groups.Any(group => IsSid(group)
            ? principal.HasClaim(c =>
                (c.Type == ClaimTypes.GroupSid || c.Type == ClaimTypes.PrimaryGroupSid) &&
                string.Equals(c.Value, group, StringComparison.OrdinalIgnoreCase))
            : IsInRoleSafe(principal, group));

    /// <summary>
    /// Buscar un grupo por nombre requiere consultar al controlador de dominio. Si el grupo
    /// no existe o el dominio no responde, se considera que el usuario NO pertenece
    /// (se deniega, nunca se concede) y se deja una advertencia en el log.
    /// </summary>
    private bool IsInRoleSafe(ClaimsPrincipal principal, string group)
    {
        try
        {
            return principal.IsInRole(group);
        }
        catch (IdentityNotMappedException ex)
        {
            LogGroupNotResolved(logger, ex, group);
            return false;
        }
        catch (Win32Exception ex)
        {
            LogGroupNotResolved(logger, ex, group);
            return false;
        }
    }

    private static bool IsSid(string value) => value.StartsWith("S-1-", StringComparison.OrdinalIgnoreCase);

    [LoggerMessage(
        EventId = 4010,
        Level = LogLevel.Warning,
        Message = "No se pudo resolver el grupo de AD '{Group}'; se trata como no miembro. Considera configurarlo por SID.")]
    private static partial void LogGroupNotResolved(ILogger logger, Exception exception, string group);
}
