using MediatR;
using NexTube.Application.Common.Interfaces;
using NexTube.Application.CQRS.Videos.Notifications.VideoCreated;
using NexTube.Application.Models;
using NexTube.Application.Models.Lookups;
using NexTube.Domain.Entities;

namespace NexTube.Application.CQRS.Videos.Notifications.VideoUploading {
    public class VideoUploadingRealTimeNotificationHandler : INotificationHandler<VideoUploadingNotification> {
        private readonly IEventPublisher<VideoUploadProgress> _eventPublisher;
        private readonly IDateTimeService _dateTimeService;

        public VideoUploadingRealTimeNotificationHandler(IEventPublisher<VideoUploadProgress> eventPublisher, IDateTimeService dateTimeService) {
            _eventPublisher = eventPublisher;
            _dateTimeService = dateTimeService;
        }

        public async Task Handle(VideoUploadingNotification notification, CancellationToken cancellationToken) {
            await _eventPublisher.SendEvent(notification.Progress);
        }
    }
}
