using MongoDB.Driver;
using Newtonsoft.Json;
using Microsoft.AspNetCore.SignalR;
using Myrtus.Clarity.Core.Infrastructure.SignalR.Hubs;
using Microsoft.Extensions.Configuration;
using Myrtus.Clarity.Core.Application.Abstractions.Auditing;
using Myrtus.Clarity.Core.Domain.Abstractions;

public class AuditLogService : IAuditLogService
{
    private readonly IMongoCollection<AuditLog> _auditLogs;
    private readonly IHubContext<AuditLogHub> _hubContext;

    public AuditLogService(IConfiguration configuration, IHubContext<AuditLogHub> hubContext)
    {
        var client = new MongoClient(configuration.GetConnectionString("MongoDb"));
        var database = client.GetDatabase(configuration["MongoDb:Database"]);
        _auditLogs = database.GetCollection<AuditLog>("AuditLogs");
        _hubContext = hubContext;
    }

    public async Task LogAsync(AuditLog log)
    {
        await _auditLogs.InsertOneAsync(log);
        var message = JsonConvert.SerializeObject(log);
        await _hubContext.Clients.All.SendAsync("ReceiveAuditLog", message);
    }
}
