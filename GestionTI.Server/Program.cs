using GestionTI.Application;
using GestionTI.Application.Security;
using GestionTI.Infrastructure;
using GestionTI.Server.Errors;
using GestionTI.Server.Security;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

// No anunciar el servidor web en las respuestas.
builder.WebHost.ConfigureKestrel(options => options.AddServerHeader = false);

// Capas de la solución.
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddSingleton(TimeProvider.System);

// Autenticación de Windows (Kerberos/NTLM). En IIS se usa la autenticación de Windows del propio IIS.
builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme).AddNegotiate();

// Roles a partir de grupos de Active Directory y políticas según la matriz de permisos.
builder.Services.Configure<SecurityOptions>(builder.Configuration.GetSection(SecurityOptions.SectionName));
builder.Services.AddTransient<IClaimsTransformation, AdGroupRoleClaimsTransformation>();
builder.Services.AddSingleton<IAuthorizationMiddlewareResultHandler, AccessDeniedAuditHandler>();
builder.Services.AddAuthorizationBuilder()
    .AddPolicy(AppPolicies.Read, p => p.RequireRole(AppRoles.Admin, AppRoles.Technician, AppRoles.ReadOnly))
    .AddPolicy(AppPolicies.Operate, p => p.RequireRole(AppRoles.Admin, AppRoles.Technician))
    .AddPolicy(AppPolicies.Administer, p => p.RequireRole(AppRoles.Admin))
    // Todo lo que no declare una política exige, al menos, haber iniciado sesión.
    .SetFallbackPolicy(new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build());

// Errores como ProblemDetails, sin detalles internos.
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

// Encabezados de seguridad básicos. Se agregan justo antes de enviar la respuesta
// para que también vayan en las respuestas de error.
app.Use(async (context, next) =>
{
    context.Response.OnStarting(() =>
    {
        var headers = context.Response.Headers;
        headers.XContentTypeOptions = "nosniff";
        headers.XFrameOptions = "DENY";
        headers["Referrer-Policy"] = "no-referrer";
        return Task.CompletedTask;
    });
    await next(context);
});

app.UseExceptionHandler();
app.UseStatusCodePages();

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseDefaultFiles();
app.MapStaticAssets();

app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapControllers();

// Una ruta /api que no existe responde 404 (no la página de Angular).
app.MapFallback("/api/{**path}", () => Results.Problem(statusCode: StatusCodes.Status404NotFound));

app.MapFallbackToFile("/index.html");

app.Run();
