using GestionTI.Application.Exceptions;
using GestionTI.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace GestionTI.Server.Errors;

/// <summary>
/// Convierte las excepciones en respuestas ProblemDetails.
/// Nunca expone detalles internos: los errores inesperados se registran en el log
/// y el usuario solo recibe un mensaje genérico con el traceId.
/// </summary>
public sealed partial class GlobalExceptionHandler(
    IProblemDetailsService problemDetailsService,
    ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(httpContext);
        ArgumentNullException.ThrowIfNull(exception);

        var (status, title, detail) = exception switch
        {
            NotFoundException e => (StatusCodes.Status404NotFound, "No encontrado", e.Message),
            BusinessRuleException e => (StatusCodes.Status409Conflict, "No se puede completar la operación", e.Message),
            _ => (StatusCodes.Status500InternalServerError, "Error interno",
                "Ocurrió un error inesperado. Si continúa, comunícate con el área de TI y comparte el traceId."),
        };

        if (status == StatusCodes.Status500InternalServerError)
        {
            LogUnhandledException(logger, exception, httpContext.TraceIdentifier);
        }

        httpContext.Response.StatusCode = status;
        return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            ProblemDetails = new ProblemDetails { Status = status, Title = title, Detail = detail },
        });
    }

    [LoggerMessage(EventId = 5000, Level = LogLevel.Error, Message = "Error no controlado (traceId {TraceId})")]
    private static partial void LogUnhandledException(ILogger logger, Exception exception, string traceId);
}
