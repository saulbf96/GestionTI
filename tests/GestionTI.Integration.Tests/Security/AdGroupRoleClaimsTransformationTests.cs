using System.Security.Claims;
using GestionTI.Application.Security;
using GestionTI.Server.Security;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

namespace GestionTI.Integration.Tests.Security;

public sealed class AdGroupRoleClaimsTransformationTests
{
    private static AdGroupRoleClaimsTransformation CreateTransformation()
    {
        var options = new SecurityOptions();
        options.Admin.Add(@"UNOCONTACT\GG_GestionTI_Admin");
        options.Technician.Add("S-1-5-21-1111-2222-3333-4444");
        options.ReadOnly.Add(@"UNOCONTACT\GG_GestionTI_Consulta");
        return new AdGroupRoleClaimsTransformation(
            new StaticOptionsMonitor<SecurityOptions>(options),
            NullLogger<AdGroupRoleClaimsTransformation>.Instance);
    }

    private static ClaimsPrincipal CreateUser(params Claim[] extraClaims) =>
        new(new ClaimsIdentity(
            [new Claim(ClaimTypes.Name, @"UNOCONTACT\usuario"), .. extraClaims],
            "Negotiate", ClaimTypes.Name, ClaimTypes.Role));

    [Fact]
    public async Task GrupoPorNombre_SeConvierteEnRol()
    {
        var user = CreateUser(new Claim(ClaimTypes.Role, @"UNOCONTACT\GG_GestionTI_Admin"));

        var result = await CreateTransformation().TransformAsync(user);

        Assert.True(result.IsInRole(AppRoles.Admin));
        Assert.False(result.IsInRole(AppRoles.Technician));
        Assert.False(result.IsInRole(AppRoles.ReadOnly));
    }

    [Fact]
    public async Task GrupoPorSid_SeConvierteEnRol()
    {
        var user = CreateUser(new Claim(ClaimTypes.GroupSid, "S-1-5-21-1111-2222-3333-4444"));

        var result = await CreateTransformation().TransformAsync(user);

        Assert.True(result.IsInRole(AppRoles.Technician));
        Assert.False(result.IsInRole(AppRoles.Admin));
    }

    [Fact]
    public async Task SinGruposDeLaAplicacion_NoTieneRoles()
    {
        var user = CreateUser(new Claim(ClaimTypes.Role, @"UNOCONTACT\Otro_Grupo"));

        var result = await CreateTransformation().TransformAsync(user);

        Assert.DoesNotContain(AppRoles.All, result.IsInRole);
    }

    [Fact]
    public async Task UsuarioAnonimo_NoSeModifica()
    {
        var anonymous = new ClaimsPrincipal(new ClaimsIdentity());

        var result = await CreateTransformation().TransformAsync(anonymous);

        Assert.Same(anonymous, result);
    }

    [Fact]
    public async Task SegundaTransformacion_NoDuplicaRoles()
    {
        var transformation = CreateTransformation();
        var user = CreateUser(new Claim(ClaimTypes.Role, @"UNOCONTACT\GG_GestionTI_Admin"));

        var once = await transformation.TransformAsync(user);
        var twice = await transformation.TransformAsync(once);

        Assert.Same(once, twice);
        Assert.Single(twice.FindAll(c => c.Type == ClaimTypes.Role && c.Value == AppRoles.Admin));
    }

    private sealed class StaticOptionsMonitor<T>(T value) : IOptionsMonitor<T>
    {
        public T CurrentValue => value;

        public T Get(string? name) => value;

        public IDisposable? OnChange(Action<T, string?> listener) => null;
    }
}
