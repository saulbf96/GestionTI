namespace GestionTI.Application.Exceptions;

/// <summary>
/// Se lanza cuando el registro solicitado no existe (se responde 404).
/// </summary>
public class NotFoundException : Exception
{
    public NotFoundException()
    {
    }

    public NotFoundException(string message)
        : base(message)
    {
    }

    public NotFoundException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
