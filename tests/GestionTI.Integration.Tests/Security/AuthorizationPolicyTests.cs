using System.Security.Claims;
using GestionTI.Application.Security;
using GestionTI.Integration.Tests.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace GestionTI.Integration.Tests.Security;

/// <summary>
/// Verifica la matriz de permisos del CLAUDE.md: rol × política.
/// </summary>
public sealed class AuthorizationPolicyTests(GestionTIApiFactory factory) : IClassFixture<GestionTIApiFactory>
{
    [Theory]
    [InlineData(AppRoles.Admin, AppPolicies.Read, true)]
    [InlineData(AppRoles.Admin, AppPolicies.Operate, true)]
    [InlineData(AppRoles.Admin, AppPolicies.Administer, true)]
    [InlineData(AppRoles.Technician, AppPolicies.Read, true)]
    [InlineData(AppRoles.Technician, AppPolicies.Operate, true)]
    [InlineData(AppRoles.Technician, AppPolicies.Administer, false)]
    [InlineData(AppRoles.ReadOnly, AppPolicies.Read, true)]
    [InlineData(AppRoles.ReadOnly, AppPolicies.Operate, false)]
    [InlineData(AppRoles.ReadOnly, AppPolicies.Administer, false)]
    public async Task Politica_SegunRol_PermiteODeniega(string role, string policy, bool expected)
    {
        var authorization = factory.Services.GetRequiredService<IAuthorizationService>();
        var user = new ClaimsPrincipal(new ClaimsIdentity(
            [new Claim(ClaimTypes.Name, "TEST\\usuario"), new Claim(ClaimTypes.Role, role)],
            "Test", ClaimTypes.Name, ClaimTypes.Role));

        var result = await authorization.AuthorizeAsync(user, policy);

        Assert.Equal(expected, result.Succeeded);
    }

    [Theory]
    [InlineData(AppPolicies.Read)]
    [InlineData(AppPolicies.Operate)]
    [InlineData(AppPolicies.Administer)]
    public async Task Politica_UsuarioSinRol_SeDeniega(string policy)
    {
        var authorization = factory.Services.GetRequiredService<IAuthorizationService>();
        var user = new ClaimsPrincipal(new ClaimsIdentity(
            [new Claim(ClaimTypes.Name, "TEST\\usuario")], "Test", ClaimTypes.Name, ClaimTypes.Role));

        var result = await authorization.AuthorizeAsync(user, policy);

        Assert.False(result.Succeeded);
    }
}
