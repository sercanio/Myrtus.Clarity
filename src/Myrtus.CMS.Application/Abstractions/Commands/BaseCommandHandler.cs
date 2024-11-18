using Ardalis.Result;
using Microsoft.AspNetCore.Http;
using Myrtus.Clarity.Core.Application.Abstractions.Auditing;
using Myrtus.Clarity.Core.Application.Abstractions.Messaging;
using Myrtus.Clarity.Core.Domain.Abstractions;
using System.Security.Claims;

namespace Myrtus.Clarity.Core.Application.Abstractions.Commands;

public abstract class BaseCommandHandler<TRequest, TResponse> : ICommandHandler<TRequest, TResponse>
    where TRequest : ICommand<TResponse>
{
    private readonly IAuditLogService _auditLogService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    protected BaseCommandHandler(IAuditLogService auditLogService, IHttpContextAccessor httpContextAccessor)
    {
        _auditLogService = auditLogService;
        _httpContextAccessor = httpContextAccessor;
    }

    public abstract Task<Result<TResponse>> Handle(TRequest request, CancellationToken cancellationToken);

    protected async Task LogAuditAsync(string action, string entity, string entityId, string details)
    {
        var emailAddress = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value;
        var auditLog = new AuditLog
        {
            User = emailAddress ?? "system",
            Action = action,
            Entity = entity,
            EntityId = entityId,
            Details = details,
        };
        await _auditLogService.LogAsync(auditLog);
    }
}
