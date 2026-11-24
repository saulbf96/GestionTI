namespace GestionTI.Server.Security;

/// <summary>
/// Grupos de Active Directory que otorgan cada rol (sección "Security:Groups").
/// Cada entrada puede ser un nombre ("UNOCONTACT\GG_GestionTI_Admin") o un SID ("S-1-5-...").
/// </summary>
public sealed class SecurityOptions
{
    public const string SectionName = "Security:Groups";

    public IList<string> Admin { get; } = new List<string>();

    public IList<string> Technician { get; } = new List<string>();

    public IList<string> ReadOnly { get; } = new List<string>();
}
