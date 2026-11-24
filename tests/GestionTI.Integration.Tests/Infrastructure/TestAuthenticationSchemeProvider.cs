using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.Extensions.Options;

namespace GestionTI.Integration.Tests.Infrastructure;

/// <summary>
/// El handler de Negotiate (Windows) se ejecuta en cada petición y solo funciona con Kestrel.
/// El servidor en memoria de las pruebas no es Kestrel, así que aquí se excluye.
/// </summary>
public sealed class TestAuthenticationSchemeProvider(IOptions<AuthenticationOptions> options)
    : AuthenticationSchemeProvider(options)
{
    public override async Task<IEnumerable<AuthenticationScheme>> GetRequestHandlerSchemesAsync() =>
        (await base.GetRequestHandlerSchemesAsync())
            .Where(scheme => scheme.Name != NegotiateDefaults.AuthenticationScheme);
}
