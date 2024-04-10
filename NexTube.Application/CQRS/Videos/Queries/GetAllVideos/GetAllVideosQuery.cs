using MediatR;

namespace NexTube.Application.CQRS.Videos.Queries.GetAllVideos {
    public class GetAllVideosQuery : IRequest<GetAllVideosQueryResult> {
        public string? Name { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
