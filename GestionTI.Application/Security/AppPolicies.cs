namespace GestionTI.Application.Security;

/// <summary>
/// Políticas de autorización según la matriz de permisos del CLAUDE.md.
/// Se validan siempre en la API; Angular solo oculta menús y botones.
/// </summary>
public static class AppPolicies
{
    /// <summary>Dashboard, ver usuarios/inventario, historial y exportar. Admin, Técnico y Consulta.</summary>
    public const string Read = "Read";

    /// <summary>Alta de colaborador, alta/edición de equipo, asignar, devolver y responsivas. Admin y Técnico.</summary>
    public const string Operate = "Operate";

    /// <summary>Bajas, catálogos, roles y permisos. Solo Admin.</summary>
    public const string Administer = "Administer";
}
