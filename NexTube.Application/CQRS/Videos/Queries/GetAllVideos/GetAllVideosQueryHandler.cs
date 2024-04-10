using MediatR;
using Microsoft.EntityFrameworkCore;
using NexTube.Application.Common.DbContexts;
using NexTube.Application.Models.Lookups;
using NexTube.Domain.Constants;

namespace NexTube.Application.CQRS.Videos.Queries.GetAllVideos {
    public class GetAllVideosQueryHandler : IRequestHandler<GetAllVideosQuery, GetAllVideosQueryResult> {
        private readonly IApplicationDbContext _dbContext;

        public GetAllVideosQueryHandler(IApplicationDbContext dbContext) {
            _dbContext = dbContext;
        }

        public async Task<GetAllVideosQueryResult> Handle(GetAllVideosQuery request, CancellationToken cancellationToken) {
            var videoLookups = await _dbContext.Videos
               .OrderByDescending(c => c.DateCreated)
               .Include(e => e.Creator)
               .Skip(( request.Page - 1 ) * request.PageSize)
               .Take(request.PageSize)
               .Select(v => new VideoLookup() {
                   Id = v.Id,
                   Name = v.Name,
                   Description = v.Description,
                   VideoFile = v.VideoFileId,
                   AccessModificator = v.AccessModificator.Modificator,
                   PreviewPhotoFile = v.PreviewPhotoFileId,
                   DateCreated = v.DateCreated,
                   Views = v.Views,
                   DateModified = v.DateModified,
                   Creator = new UserLookup() {
                       UserId = v.Creator.Id,
                       FirstName = v.Creator.FirstName,
                       LastName = v.Creator.LastName,
                       ChannelPhoto = v.Creator.ChannelPhotoFileId.ToString(),
                   },
                   IsApproved = v.IsApproved,
               })
               .ToListAsync();

            var GetAllVideoEntitiesQueryResult = new GetAllVideosQueryResult() {
                Videos = videoLookups,
            };

            return GetAllVideoEntitiesQueryResult;
        }
    }
}
