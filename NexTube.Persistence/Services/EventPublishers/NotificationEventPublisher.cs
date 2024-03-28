using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using NexTube.Application.Common.DbContexts;
using NexTube.Application.Common.Interfaces;
using NexTube.Application.Models.Lookups;
using NexTube.Infrastructure.Hubs;

namespace NexTube.Persistence.Services.EventPublishers {
    public class NotificationEventPublisher : IEventPublisher<NotificationLookup> {
        private readonly IHubContext<NotificationsHub> _hub;
        private readonly IApplicationDbContext _db;

        public NotificationEventPublisher(IHubContext<NotificationsHub> hubContext, IApplicationDbContext db) {
            _hub = hubContext;
            _db = db;
        }

        public async Task SendEvent(NotificationLookup data) {
            // take a subscribers list of user that created current video
            var result = await _db.Subscriptions
                .Where(s => s.SubscriberId == data.NotificationIssuer.UserId) // get list of user subscribers
                .Select(s => s.CreatorId!.ToString() ?? "")
                .ToListAsync();

            // and notify them about the event
            await _hub.Clients
                .Users(result)
                .SendAsync("OnNotificationReceived", data);
        }
    }
}
