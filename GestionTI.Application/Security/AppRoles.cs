namespace GestionTI.Application.Security;

/// <summary>
/// Roles de la aplicación. Cada uno se obtiene de un grupo de Active Directory
/// (configurado en "Security:Groups").
/// </summary>
public static class AppRoles
{
    /// <summary>Jefe de TI: acceso total, incluidas bajas, catálogos y roles.</summary>
    public const string Admin = "Admin";

    /// <summary>Técnico de TI: altas, asignaciones, devoluciones, responsivas e inventario.</summary>
    public const string Technician = "Technician";

    /// <summary>Jefes de área y auditoría: solo lectura y exportación.</summary>
    public const string ReadOnly = "ReadOnly";

    public static IReadOnlyList<string> All { get; } = [Admin, Technician, ReadOnly];
}
