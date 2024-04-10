using MediatR;
using NexTube.Application.Common.Interfaces;

namespace NexTube.Application.CQRS.Videos.Commands.ApproveVideo {
    public class ApproveVideoCommand : IRequest {
        public int VideoId { get; set; }
    }
}
