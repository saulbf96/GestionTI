namespace GestionTI.Server.Contracts;

/// <param name="UserName">Cuenta de red del usuario (UNOCONTACT\usuario).</param>
/// <param name="Roles">Roles de la aplicación que tiene el usuario.</param>
public sealed record SessionResponse(string UserName, IReadOnlyList<string> Roles);
