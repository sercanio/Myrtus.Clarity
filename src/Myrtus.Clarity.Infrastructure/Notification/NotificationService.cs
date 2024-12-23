using Microsoft.AspNetCore.SignalR;
using MongoDB.Driver;
using Myrtus.Clarity.Application.Repositories.NoSQL;
using Myrtus.Clarity.Application.Services.Users;
using Myrtus.Clarity.Core.Application.Abstractions.Notification;
using Myrtus.Clarity.Core.Domain.Abstractions;
using Myrtus.Clarity.Core.Infrastructure.SignalR.Hubs;
using Myrtus.Clarity.Domain.Users;

namespace Myrtus.Clarity.Infrastructure.Notifications.Services
{
    public class NotificationService : INotificationService, IDisposable
    {
        private readonly INoSqlRepository<Notification> _notificationRepository;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly IUserService _userService; // Added IUserService dependency
        private bool _disposed;

        public NotificationService(
            INoSqlRepository<Notification> notificationRepository,
            IHubContext<NotificationHub> hubContext,
            IUserService userService) // Injected IUserService
        {
            _notificationRepository = notificationRepository;
            _hubContext = hubContext;
            _userService = userService;
        }

        private static Notification CreateNotification(string? userId = null, string? user = null, string? action = null, string? entity = null, string? entityId = null, string? details = null)
        {
            return new Notification
            {
                Id = Guid.NewGuid(),
                UserId = userId ?? string.Empty,
                User = user ?? string.Empty,
                Action = action ?? string.Empty,
                Entity = entity ?? string.Empty,
                EntityId = entityId ?? string.Empty,
                Timestamp = DateTime.UtcNow,
                Details = details ?? string.Empty,
                IsRead = false
            };
        }

        private async Task InsertNotificationAsync(Notification notification)
        {
            await _notificationRepository.AddAsync(notification);
        }

        public async Task SendNotificationAsync(string details)
        {
            Notification notification = CreateNotification(details: details);
            await InsertNotificationAsync(notification);
            await _hubContext.Clients.All.SendAsync("ReceiveNotification", notification);
        }

        public async Task SaveNotificationAsync(string details, string userId)
        {
            Notification notification = CreateNotification(userId: userId, details: details);
            await InsertNotificationAsync(notification);
        }

        public async Task SendNotificationToUserAsync(string details, string userId)
        {
            Notification notification = CreateNotification(userId: userId, details: details);
            await InsertNotificationAsync(notification);
            await _hubContext.Clients.User(userId).SendAsync("ReceiveNotification", notification);
        }

        public async Task<List<Notification>> GetNotificationsByUserIdAsync(string userId)
        {
            return (await _notificationRepository.GetByPredicateAsync(n => n.UserId == userId)).ToList();
        }

        public async Task SendNotificationToUsersAsync(string details, IEnumerable<string> userIds)
        {
            List<Notification> notifications = new();
            foreach (string userId in userIds)
            {
                Notification notification = CreateNotification(userId: userId, details: details);
                notifications.Add(notification);
            }

            foreach (var notification in notifications)
            {
                await InsertNotificationAsync(notification);
                await _hubContext.Clients.User(notification.UserId).SendAsync("ReceiveNotification", notification);
            }
        }

        public async Task<IEnumerable<Notification>> GetNotificationsByUserIdsAsync(string userId, CancellationToken cancellationToken)
        {
            return await _notificationRepository.GetAllAsync(n => n.UserId == userId, cancellationToken);
        }

        public async Task SendNotificationToUserGroupAsync(string details, string groupName)
        {
            var paginatedUsers = await _userService.GetAllAsync(
                predicate: user => user.Roles.Any(role => role.Name == groupName));
            IEnumerable<User> users = paginatedUsers.Items;

            List<string> userIds = users.Select(u => u.IdentityId.ToString()).ToList();

            await SendNotificationToUsersAsync(details, userIds);
        }

        public async Task MarkNotificationsAsReadAsync(string UserId, CancellationToken cancellation)
        {
            var notifications = await _notificationRepository.GetAllAsync(
                predicate: n => n.UserId == UserId && !n.IsRead,
                cancellationToken: cancellation);

            foreach (var notification in notifications)
            {
                notification.IsRead = true;
                await _notificationRepository.UpdateAsync(notification, cancellation);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Dispose managed resources here
                }
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
