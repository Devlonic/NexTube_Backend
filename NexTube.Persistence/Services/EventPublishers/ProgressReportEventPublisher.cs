using Microsoft.AspNetCore.SignalR;
using NexTube.Application.Common.Interfaces;
using NexTube.Application.Models;
using NexTube.Infrastructure.Hubs;

namespace NexTube.Persistence.Services.EventPublishers {
    public class ProgressReportEventPublisher : IEventPublisher<VideoUploadProgress> {
        private readonly IHubContext<ProgressReportHub> _hub;

        public ProgressReportEventPublisher(IHubContext<ProgressReportHub> hubContext) {
            _hub = hubContext;
        }

        public async Task SendEvent(VideoUploadProgress data) {
            await Console.Out.WriteLineAsync($"{data.Percentage}");
            // notify operation initiator about progress status
            var creator = data.Video!.Creator!.UserId.ToString();
            data.Video = null;
            await _hub.Clients
                .User(creator)
                .SendAsync("OnProgressStatusChanged", data);
        }
    }
}
