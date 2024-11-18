using Myrtus.Clarity.Core.Domain.Abstractions;

namespace Myrtus.Clarity.Core.Application.Abstractions.Auditing;

public interface IAuditLogService
{
    Task LogAsync(AuditLog log);
}