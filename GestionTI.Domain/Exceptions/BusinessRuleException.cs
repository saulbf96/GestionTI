namespace GestionTI.Domain.Exceptions;

/// <summary>
/// Se lanza cuando una operación viola una regla de negocio
/// (por ejemplo, asignar un equipo que no está Disponible).
/// El mensaje se muestra al usuario, así que debe ir en español y sin datos internos.
/// </summary>
public class BusinessRuleException : Exception
{
    public BusinessRuleException()
    {
    }

    public BusinessRuleException(string message)
        : base(message)
    {
    }

    public BusinessRuleException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
