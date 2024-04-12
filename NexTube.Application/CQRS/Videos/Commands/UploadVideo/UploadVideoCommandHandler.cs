using Ardalis.GuardClauses;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NexTube.Application.Common.DbContexts;
using NexTube.Application.Common.Interfaces;
using NexTube.Application.CQRS.Videos.Notifications.VideoCreated;
using NexTube.Application.CQRS.Videos.Notifications.VideoUploading;
using NexTube.Application.Models;
using NexTube.Application.Models.Lookups;
using NexTube.Domain.Entities;
using System.Text.RegularExpressions;
using System.Threading.Channels;

namespace NexTube.Application.CQRS.Videos.Commands.UploadVideo {
    public class UploadVideoCommandHandler : IRequestHandler<UploadVideoCommand, VideoLookup> {
        private readonly IVideoService _videoService;
        private readonly IPhotoService _photoService;
        private readonly IDateTimeService _dateTimeService;
        private readonly IApplicationDbContext _dbContext;
        private readonly IMediator _mediator;

        public UploadVideoCommandHandler(IVideoService videoService, IPhotoService photoService, IDateTimeService dateTimeService, IApplicationDbContext dbContext, IMediator mediator) {
            _videoService = videoService;
            _photoService = photoService;
            _dateTimeService = dateTimeService;
            _dbContext = dbContext;
            _mediator = mediator;
        }

        public async Task<VideoLookup> Handle(UploadVideoCommand request, CancellationToken cancellationToken) {
            request.Description = Regex.Replace(request.Description, @"<", "&lt;");
            request.Description = Regex.Replace(request.Description, @">", "&gt;");

            var video = new VideoEntity() {
                Name = request.Name,
                Description = request.Description,
                VideoFileId = null,
                PreviewPhotoFileId = null,
                Creator = request.Creator,
                AccessModificator = null,
                DateCreated = _dateTimeService.Now,
            };
            _dbContext.Videos.Add(video);
            await _dbContext.SaveChangesAsync(cancellationToken);

            var videoLookup = new VideoLookup() {
                Id = video.Id,
                Name = video.Name,
                Description = video.Description,
                DateCreated = video.DateCreated,
                DateModified = video.DateModified,
                Views = video.Views,
                Creator = new UserLookup() {
                    UserId = video.Creator!.Id,
                    FirstName = video.Creator.FirstName,
                    LastName = video.Creator.LastName,
                    ChannelPhoto = video.Creator.ChannelPhotoFileId.ToString(),
                }
            };

            var progressTracker = new Progress<FileUploadProgress>(async (report) => {
                var videoUploadProgress = new VideoUploadProgress() {
                    Percentage = report.Percentage,
                    TotalBytesTransferred = report.TotalBytesTransferred,
                    Video = videoLookup
                };

                await _mediator.Publish(new VideoUploadingNotification() {
                    Progress = videoUploadProgress
                });
            });

            var videoUploadResult = await _videoService.UploadVideoAsync(request.Source, progressTracker);
            video.VideoFileId = Guid.Parse(videoUploadResult.VideoFileId);

            var photoUploadResult = await _photoService.UploadPhoto(request.PreviewPhotoSource);
            video.PreviewPhotoFileId = Guid.Parse(photoUploadResult.PhotoId);

            var accessModificator = await _dbContext.VideoAccessModificators.Where(v => v.Modificator == request.AccessModificator).FirstOrDefaultAsync();
            video.AccessModificator = accessModificator;

            if ( accessModificator == null ) {
                throw new NotFoundException(request.AccessModificator, nameof(VideoAccessModificatorEntity));
            }

            videoLookup.AccessModificator = video.AccessModificator.Modificator;
            videoLookup.PreviewPhotoFile = video.PreviewPhotoFileId;
            videoLookup.VideoFile = video.VideoFileId;

            _dbContext.Videos.Update(video);
            await _dbContext.SaveChangesAsync(cancellationToken);

            if ( video.AccessModificator.Modificator.ToLower() != "private" ) {
                await _mediator.Publish(new VideoCreatedNotification() {
                    Video = video,
                });
            }

            return videoLookup;
        }
    }
}
