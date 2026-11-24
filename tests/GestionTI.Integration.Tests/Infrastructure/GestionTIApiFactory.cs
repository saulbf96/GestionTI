using GestionTI.Application.Security;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GestionTI.Integration.Tests.Infrastructure;

/// <summary>
/// Levanta la API en memoria con autenticación simulada y grupos de AD de prueba.
/// </summary>
public sealed class GestionTIApiFactory : WebApplicationFactory<Program>
{
    public const string AdminGroup = @"TEST\GG_GestionTI_Admin";
    public const string TechnicianGroup = @"TEST\GG_GestionTI_Tecnico";
    public const string ReadOnlyGroup = @"TEST\GG_GestionTI_Consulta";

    public RecordingAccessDeniedLog AccessDeniedLog { get; } = new();

    public HttpClient CreateClientAs(string user, params string[] groups)
    {
        var client = CreateClient();
        client.DefaultRequestHeaders.Add(TestAuthHandler.UserHeader, user);
        if (groups.Length > 0)
        {
            client.DefaultRequestHeaders.Add(TestAuthHandler.GroupsHeader, string.Join(',', groups));
        }

        return client;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureAppConfiguration((_, config) => config.AddInMemoryCollection(new Dictionary<string, string?>
        {
            ["Security:Groups:Admin:0"] = AdminGroup,
            ["Security:Groups:Technician:0"] = TechnicianGroup,
            ["Security:Groups:ReadOnly:0"] = ReadOnlyGroup,
        }));

        builder.ConfigureTestServices(services =>
        {
            services.AddAuthentication(options =>
                {
                    options.DefaultScheme = TestAuthHandler.SchemeName;
                    options.DefaultAuthenticateScheme = TestAuthHandler.SchemeName;
                    options.DefaultChallengeScheme = TestAuthHandler.SchemeName;
                })
                .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(TestAuthHandler.SchemeName, _ => { });
            services.AddSingleton<IAuthenticationSchemeProvider, TestAuthenticationSchemeProvider>();

            services.AddSingleton<IAccessDeniedLog>(AccessDeniedLog);
        });
    }
}
