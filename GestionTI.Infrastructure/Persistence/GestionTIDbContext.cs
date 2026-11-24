using Microsoft.EntityFrameworkCore;

namespace GestionTI.Infrastructure.Persistence;

/// <summary>
/// Contexto de EF Core. La estructura de la base de datos NO se crea desde aquí:
/// la controlan los scripts de GESTION_TI\SCRIPTS (sin migraciones de EF).
/// </summary>
public sealed class GestionTIDbContext(DbContextOptions<GestionTIDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);

        // Configuraciones de cada entidad (IEntityTypeConfiguration) en este ensamblado.
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(GestionTIDbContext).Assembly);
    }
}
