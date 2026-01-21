namespace Catalog.API.Repositories;

public class AuditRepository : IAuditRepository
{
    private readonly ApplicationDbContext _context;

    public AuditRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddLogAsync(Guid plateId, string action, string details)
    {
        var log = new AuditLog
        {
            AuditId = Guid.NewGuid(),
            PlateId = plateId,
            Action = action,
            Details = details,
            Timestamp = DateTime.UtcNow
        };

        await _context.AuditLogs.AddAsync(log);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<AuditLog>> GetHistoryAsync(Guid plateId)
    {
        return await _context.AuditLogs
            .AsNoTracking() // as same behaviour as revenue stats
            .Where(x => x.PlateId == plateId)
            .OrderByDescending(x => x.Timestamp)
            .ToListAsync();
    }
}
