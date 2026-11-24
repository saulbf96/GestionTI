using System.Net;
using System.Net.Http.Json;
using GestionTI.Application.Security;
using GestionTI.Integration.Tests.Infrastructure;
using GestionTI.Server.Contracts;

namespace GestionTI.Integration.Tests.Security;

public sealed class ApiSecurityTests(GestionTIApiFactory factory) : IClassFixture<GestionTIApiFactory>
{
    [Fact]
    public async Task Session_SinIniciarSesion_Responde401()
    {
        var client = factory.CreateClient();

        var response = await client.GetAsync(new Uri("/api/session", UriKind.Relative));

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Session_UsuarioDelGrupoAdmin_DevuelveRolAdmin()
    {
        var client = factory.CreateClientAs(@"TEST\jperez", GestionTIApiFactory.AdminGroup);

        var session = await client.GetFromJsonAsync<SessionResponse>(new Uri("/api/session", UriKind.Relative));

        Assert.NotNull(session);
        Assert.Equal(@"TEST\jperez", session.UserName);
        Assert.Equal([AppRoles.Admin], session.Roles);
    }

    [Fact]
    public async Task Session_UsuarioDeConsulta_DevuelveSoloRolConsulta()
    {
        var client = factory.CreateClientAs(@"TEST\rsalinas", GestionTIApiFactory.ReadOnlyGroup);

        var session = await client.GetFromJsonAsync<SessionResponse>(new Uri("/api/session", UriKind.Relative));

        Assert.NotNull(session);
        Assert.Equal([AppRoles.ReadOnly], session.Roles);
    }

    [Fact]
    public async Task Session_UsuarioSinGrupoDeLaAplicacion_Responde403YQuedaEnBitacora()
    {
        var client = factory.CreateClientAs(@"TEST\sinpermiso", @"TEST\Otro_Grupo");

        var response = await client.GetAsync(new Uri("/api/session", UriKind.Relative));

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        var entry = Assert.Single(factory.AccessDeniedLog.Entries, e => e.UserName == @"TEST\sinpermiso");
        Assert.Equal("GET", entry.Method);
        Assert.Equal("/api/session", entry.Resource);
    }

    [Fact]
    public async Task RutaApiInexistente_Responde404ComoProblemDetails()
    {
        var client = factory.CreateClientAs(@"TEST\jperez", GestionTIApiFactory.AdminGroup);

        var response = await client.GetAsync(new Uri("/api/no-existe", UriKind.Relative));

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
    }

    [Fact]
    public async Task Respuestas_IncluyenEncabezadosDeSeguridad()
    {
        var client = factory.CreateClientAs(@"TEST\jperez", GestionTIApiFactory.AdminGroup);

        var response = await client.GetAsync(new Uri("/api/session", UriKind.Relative));

        Assert.Equal("nosniff", response.Headers.GetValues("X-Content-Type-Options").Single());
        Assert.Equal("DENY", response.Headers.GetValues("X-Frame-Options").Single());
        Assert.Equal("no-referrer", response.Headers.GetValues("Referrer-Policy").Single());
        Assert.False(response.Headers.Contains("Server"));
    }
}
