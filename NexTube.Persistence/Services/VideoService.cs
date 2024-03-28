using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using NexTube.Application.Common.Interfaces;
using NexTube.Application.Common.Models;
using NexTube.Application.Models;
using NexTube.Application.Models.Lookups;
using NexTube.Domain.Entities;
using NexTube.Infrastructure.Services;
using NexTube.Persistence.Data.Contexts;

namespace NexTube.Persistence.Services {
    public class VideoService : IVideoService {
        private readonly IFileService _fileService;

        public VideoService(IFileService fileService) {
            _fileService = fileService;
        }

        public async Task<(Result Result, string VideoFileId)> UploadVideoAsync(Stream source) {
            var uploadVideo = await _fileService.UploadFileAsync("videos", source);
            return (uploadVideo.Result, uploadVideo.FileId);
        }
        public async Task<(Result Result, string VideoFileId)> UploadVideoAsync(Stream source, IProgress<FileUploadProgress> progress) {
            var uploadVideo = await _fileService.UploadFileAsync("videos", source, progress);
            return (uploadVideo.Result, uploadVideo.FileId);
        }

        public async Task<(Result Result, string VideoUrl)> GetUrlVideoAsync(string videoFileId) {
            var getVideo = await _fileService.GetFileUrlAsync("videos", videoFileId, "video/mp4");
            return (getVideo.Result, getVideo.Url);
        }


        public async Task<Result> DeleteVideoAsync(string videoFileId) {
            await _fileService.DeleteFileAsync("videos", videoFileId);
            return Result.Success();
        }

        public async Task<bool> IsVideoExists(string videoFileId) => await _fileService.IsFileExistsAsync("videos", videoFileId);
    }
}
