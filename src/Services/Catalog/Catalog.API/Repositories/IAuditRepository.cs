namespace Catalog.API.Repositories;

public interface IAuditRepository
{
    Task AddLogAsync(Guid plateId, string action, string details);
    Task<IEnumerable<AuditLog>> GetHistoryAsync(Guid plateId);
}
