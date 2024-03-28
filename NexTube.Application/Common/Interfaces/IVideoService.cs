using NexTube.Application.Common.Models;
using NexTube.Application.Models;
using NexTube.Application.Models.Lookups;
using NexTube.Domain.Entities;

namespace NexTube.Application.Common.Interfaces {
    public interface IVideoService {
        Task<(Result Result, string VideoFileId)> UploadVideoAsync(Stream source);
        Task<(Result Result, string VideoFileId)> UploadVideoAsync(Stream source, IProgress<FileUploadProgress> progress);
        Task<(Result Result, string VideoUrl)> GetUrlVideoAsync(string videoFileId);
        Task<Result> DeleteVideoAsync(string videoFileId);
        Task<bool> IsVideoExists(string videoFileId);
    }
}
