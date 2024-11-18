using Microsoft.AspNetCore.SignalR;

namespace Myrtus.Clarity.Core.Infrastructure.SignalR.Hubs;

public class AuditLogHub : Hub
{
    public async Task SendAuditLog(string message)
    {
        await Clients.All.SendAsync("ReceiveAuditLog", message);
    }
}
