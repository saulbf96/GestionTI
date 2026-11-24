using System.Collections.Concurrent;
using GestionTI.Application.Security;

namespace GestionTI.Integration.Tests.Infrastructure;

/// <summary>
/// Guarda en memoria los accesos denegados para verificarlos en las pruebas.
/// </summary>
public sealed class RecordingAccessDeniedLog : IAccessDeniedLog
{
    private readonly ConcurrentQueue<AccessDeniedEntry> _entries = new();

    public IReadOnlyCollection<AccessDeniedEntry> Entries => _entries.ToArray();

    public Task RecordAsync(AccessDeniedEntry entry, CancellationToken cancellationToken)
    {
        _entries.Enqueue(entry);
        return Task.CompletedTask;
    }
}
