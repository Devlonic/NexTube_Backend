using NexTube.Application.Common.Models;
using NexTube.Application.Models;

namespace NexTube.Application.Common.Interfaces {
    public interface IFileService {
        Task<(Result Result, string? FileId)> UploadFileAsync(string bucket, Stream source);
        Task<(Result Result, string? FileId)> UploadFileAsync(string bucket, Stream source, string filename);
        Task<(Result Result, string? FileId)> UploadFileAsync(string bucket, Stream source, IProgress<FileUploadProgress> progress);
        Task<(Result Result, string Url)> GetFileUrlAsync(string bucket, string fileId, string contentType);
        Task DeleteFileAsync(string bucket, string filename);
        Task<bool> IsFileExistsAsync(string bucket, string filename);
    }
}
