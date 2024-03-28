using MediatR;
using NexTube.Application.Models;

namespace NexTube.Application.CQRS.Videos.Notifications.VideoUploading {
    public class VideoUploadingNotification : INotification {
        public VideoUploadProgress Progress { get; set; } = null!;
    }
}
