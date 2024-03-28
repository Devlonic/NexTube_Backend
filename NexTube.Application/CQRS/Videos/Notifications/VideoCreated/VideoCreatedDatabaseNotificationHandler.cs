using MediatR;
using NexTube.Application.Common.DbContexts;
using NexTube.Application.Common.Interfaces;
using NexTube.Application.Models.Lookups;
using NexTube.Domain.Entities;

namespace NexTube.Application.CQRS.Videos.Notifications.VideoCreated {
    public class VideoCreatedDatabaseNotificationHandler : INotificationHandler<VideoCreatedNotification> {
        private readonly IApplicationDbContext _context;
        private readonly IDateTimeService _dateTimeService;

        public VideoCreatedDatabaseNotificationHandler(IApplicationDbContext context, IDateTimeService dateTimeService) {
            _context = context;
            _dateTimeService = dateTimeService;
        }

        public async Task Handle(VideoCreatedNotification notification, CancellationToken cancellationToken) {
            var notificationEntity = new NotificationEntity() {
                DateCreated = _dateTimeService.Now,
                NotificationIssuer = notification.Video.Creator,
                NotificationIssuerId = notification.Video.CreatorId,
                Type = NotificationEntity.NotificationType.NewVideo,
                NotificationData = notification.Video
            };

            _context.Notifications.Add(notificationEntity);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
