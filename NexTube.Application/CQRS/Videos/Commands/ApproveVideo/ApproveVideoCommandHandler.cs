﻿using Ardalis.GuardClauses;
using MediatR;
using NexTube.Application.Common.DbContexts;
using NexTube.Domain.Entities;

namespace NexTube.Application.CQRS.Videos.Commands.ApproveVideo {
    public class ApproveVideoCommandHandler : IRequestHandler<ApproveVideoCommand> {
        private readonly IApplicationDbContext _dbContext;

        public ApproveVideoCommandHandler(IApplicationDbContext dbContext) {
            _dbContext = dbContext;
        }
        public async Task Handle(ApproveVideoCommand request, CancellationToken cancellationToken) {
            var video = await _dbContext.Videos.FindAsync(new object[] { request.VideoId }, cancellationToken);
            if ( video is not null )
                video.IsApproved = true;
            else
                throw new NotFoundException(nameof(VideoEntity), request.VideoId.ToString());
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
