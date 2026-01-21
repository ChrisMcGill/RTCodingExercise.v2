namespace Catalog.API.Controllers;

[ApiController]
[Route("api/audit")]
public class AuditController : ControllerBase
{
    private readonly IAuditRepository _auditRepository;

    public AuditController(IAuditRepository auditRepository)
    {
        _auditRepository = auditRepository;
    }

    [HttpGet("{plateId}")]
    public async Task<IActionResult> GetHistory(Guid plateId)
    {
        var logs = await _auditRepository.GetHistoryAsync(plateId);
        return Ok(logs);
    }

    [HttpGet("{plateId}/export")]
    public async Task<IActionResult> ExportJson(Guid plateId)
    {
        var logs = await _auditRepository.GetHistoryAsync(plateId);

        var jsonBytes = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(logs, new System.Text.Json.JsonSerializerOptions
        {
            WriteIndented = true
        });

        return File(jsonBytes, "application/json", $"audit_history_{plateId}.json");
    }
}
