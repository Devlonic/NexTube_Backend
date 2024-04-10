using Ardalis.GuardClauses;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NexTube.Application.Common.DbContexts;
using NexTube.Application.Common.Interfaces;
using NexTube.Application.CQRS.Videos.Queries.GetRandomVideo;
using NexTube.Application.Models.Lookups;
using NexTube.Domain.Constants;
using NexTube.Domain.Entities;
using System.Linq;
using WebShop.Application.Common.Exceptions;

namespace NexTube.Application.CQRS.Videos.Queries.GetRandomVideo {
    public class GetRandomVideoQueryHandler : IRequestHandler<GetRandomVideoQuery, GetRandomVideoQueryResult> {
        private readonly IApplicationDbContext _dbContext;
        private readonly IDateTimeService _dateTimeService;

        public GetRandomVideoQueryHandler(IApplicationDbContext dbContext, IDateTimeService dateTimeService) {
            _dbContext = dbContext;
            _dateTimeService = dateTimeService;
        }

        public async Task<GetRandomVideoQueryResult> Handle(GetRandomVideoQuery request, CancellationToken cancellationToken) {
            var randomSelection = await ( from video in _dbContext.Videos
                                          where video.IsApproved == true
                                          join access in _dbContext.VideoAccessModificators on video.AccessModificator.Modificator equals access.Modificator
                                          where access.Modificator == VideoAccessModificators.Public
                                          select video ).ToListAsync();


            if ( randomSelection.Count == 0 )
                return new GetRandomVideoQueryResult() {
                    Video = new VideoLookup() {
                        Id = -1
                    }
                };


            var randomVideoId = randomSelection.ElementAt(new Random().Next(randomSelection.Count)).Id;


            var videoLookup = await _dbContext.Videos
                .Where(v => v.Id == randomVideoId)
                .Include(e => e.Creator)
                .Select(v => new VideoLookup() {
                    Id = v.Id,
                    Name = v.Name,
                    Description = v.Description,
                    VideoFile = v.VideoFileId,
                    AccessModificator = v.AccessModificator.Modificator,
                    PreviewPhotoFile = v.PreviewPhotoFileId,
                    DateCreated = v.DateCreated,
                    Views = v.Views,
                    Creator = new UserLookup() {
                        UserId = v.Creator.Id,
                        FirstName = v.Creator.FirstName,
                        LastName = v.Creator.LastName,
                        ChannelPhoto = v.Creator.ChannelPhotoFileId.ToString(),
                    },
                }).FirstOrDefaultAsync();

            if ( videoLookup == null ) {
                throw new NotFoundException(randomVideoId.ToString(), nameof(VideoEntity));
            }

            var GetVideoEntityQueryResult = new GetRandomVideoQueryResult() {
                Video = videoLookup
            };

            return GetVideoEntityQueryResult;
        }
    }
}
