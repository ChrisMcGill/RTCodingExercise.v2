using System.ComponentModel.DataAnnotations;

namespace Catalog.Domain;

public class AuditLog
{
    [Key]
    public Guid AuditId { get; set; }
    public Guid PlateId { get; set; }
    public string Action { get; set; } = string.Empty;
    public string Details { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
}
