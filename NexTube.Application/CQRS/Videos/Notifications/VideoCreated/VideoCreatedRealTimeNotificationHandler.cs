using MediatR;
using NexTube.Application.Common.DbContexts;
using NexTube.Application.Common.Interfaces;
using NexTube.Application.Models.Lookups;
using NexTube.Domain.Entities;

namespace NexTube.Application.CQRS.Videos.Notifications.VideoCreated {
    public class VideoCreatedRealTimeNotificationHandler : INotificationHandler<VideoCreatedNotification> {
        private readonly IEventPublisher<NotificationLookup> _eventPublisher;
        private readonly IDateTimeService _dateTimeService;

        public VideoCreatedRealTimeNotificationHandler(IEventPublisher<NotificationLookup> eventPublisher, IDateTimeService dateTimeService) {
            _eventPublisher = eventPublisher;
            _dateTimeService = dateTimeService;
        }

        public async Task Handle(VideoCreatedNotification notification, CancellationToken cancellationToken) {
            var notificationLookup = new NotificationLookup() {
                DateCreated = _dateTimeService.Now,
                NotificationIssuer = new UserLookup() {
                    UserId = notification.Video.Creator!.Id,
                    FirstName = notification.Video.Creator!.FirstName,
                    LastName = notification.Video.Creator!.LastName,
                    ChannelPhoto = notification.Video.Creator!.ChannelPhotoFileId!.ToString(),
                },
                Type = NotificationEntity.NotificationType.NewVideo,
                NotificationData = new VideoLookup() {
                    Id = notification.Video.Id,
                    Name = notification.Video.Name,
                    PreviewPhotoFile = notification.Video.PreviewPhotoFileId,
                }
            };

            await _eventPublisher.SendEvent(notificationLookup);
        }
    }
}
