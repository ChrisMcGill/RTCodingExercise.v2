using Catalog.API.Controllers;
using Catalog.API.Repositories;
using Catalog.Domain;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Catalog.UnitTests;

public class TestRepository : IAuditRepository
{
    private readonly List<AuditLog> _logs = new();

    public Task AddLogAsync(Guid plateId, string action, string details)
    {
        _logs.Add(new AuditLog
        {
            AuditId = Guid.NewGuid(),
            PlateId = plateId,
            Action = action,
            Details = details,
            Timestamp = DateTime.UtcNow
        });
        return Task.CompletedTask;
    }

    public Task<IEnumerable<AuditLog>> GetHistoryAsync(Guid plateId)
    {
        var result = _logs
            .Where(x => x.PlateId == plateId)
            .OrderByDescending(x => x.Timestamp)
            .AsEnumerable();

        return Task.FromResult(result);
    }
}

public class AuditTests
{
    [Fact]
    public void AuditLogStoresValuesCorrectly()
    {
        var id = Guid.NewGuid();
        var plateId = Guid.NewGuid();
        var now = DateTime.UtcNow;

        var log = new AuditLog
        {
            AuditId = id,
            PlateId = plateId,
            Action = "test action",
            Details = "test description",
            Timestamp = now
        };

        Assert.Equal(id, log.AuditId);
        Assert.Equal(plateId, log.PlateId);
        Assert.Equal("test action", log.Action);
        Assert.Equal("test description", log.Details);
        Assert.Equal(now, log.Timestamp);
    }

    [Fact]
    public async Task GetHistoryReturnsLogs()
    {
        var testRepository = new TestRepository();
        var plateId = Guid.NewGuid();

        await testRepository.AddLogAsync(plateId, "Created", "Item Created");
        await testRepository.AddLogAsync(plateId, "Updated", "Item Updated");

        var controller = new AuditController(testRepository);
        var result = await controller.GetHistory(plateId);

        var goodResult = Assert.IsType<OkObjectResult>(result);
        var logs = Assert.IsAssignableFrom<IEnumerable<AuditLog>>(goodResult.Value);

        Assert.Equal(2, logs.Count());
    }

    [Fact]
    public async Task ExportShouldReturnContent()
    {
        var testRepository = new TestRepository();
        var plateId = Guid.NewGuid();
        await testRepository.AddLogAsync(plateId, "Sold", "Plate Sold");

        var controller = new AuditController(testRepository);
        var result = await controller.ExportJson(plateId);

        var fileResult = Assert.IsType<FileContentResult>(result);
        Assert.Equal("application/json", fileResult.ContentType);
        Assert.Contains("audit_history", fileResult.FileDownloadName);

        var jsonString = System.Text.Encoding.UTF8.GetString(fileResult.FileContents);
        Assert.Contains("Plate Sold", jsonString);
    }
}
