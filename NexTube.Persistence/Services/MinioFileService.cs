using NexTube.Application.Common.Interfaces;
using NexTube.Application.Common.Models;
using Minio;
using Minio.DataModel.Args;
using Minio.Exceptions;
using Microsoft.Extensions.Configuration;

namespace NexTube.Persistence.Services {
    public class MinioFileService : IFileService {
        private readonly IMinioClient minioClient;
        private readonly bool isSslUsed;

        public MinioFileService(IMinioClient minioClient, IConfiguration configuration) {
            this.minioClient = minioClient;
            this.isSslUsed = configuration.GetValue<bool>("MinIO:SSL");
        }

        public async Task<(Result Result, string Url)> GetFileUrlAsync(string bucket, string fileId, string contentType) {
            var argsGetUrl = new PresignedGetObjectArgs()
                .WithBucket(bucket)
                .WithObject(fileId)
                .WithHeaders(new Dictionary<string, string> {
                    { "response-content-type", contentType },
                })
                .WithExpiry(7 * 24 * 3600);
            var url = await minioClient.PresignedGetObjectAsync(argsGetUrl);

            // replace https scheme to http in case of using unsecure Minio server
            if (isSslUsed == false)
                url = url.Replace("https", "http");

            return (Result.Success(), url);
        }

        public async Task<(Result Result, string? FileId)> UploadFileAsync(string bucket, Stream source) {
            return await UploadFileAsync(bucket, source, Guid.NewGuid().ToString());
        }

        public async Task<(Result Result, string? FileId)> UploadFileAsync(string bucket, Stream source, string filename) {
            var putObjArgs = new PutObjectArgs()
                .WithBucket(bucket)
                .WithObject(filename)
                .WithObjectSize(source.Length)
                .WithStreamData(source);

            var obj = await minioClient.PutObjectAsync(putObjArgs);

            return (Result.Success(), obj.ObjectName);
        }


        public async Task DeleteFileAsync(string bucket, string filename) {
            var removeObjArgs = new RemoveObjectArgs()
                .WithBucket(bucket)
                .WithObject(filename);

            await minioClient.RemoveObjectAsync(removeObjArgs);
        }

        public async Task<bool> IsFileExistsAsync(string bucket, string filename) {
            try {
                var statObjArgs = new StatObjectArgs()
                    .WithBucket(bucket)
                    .WithObject(filename);

                await minioClient.StatObjectAsync(statObjArgs);
            }
            catch (ObjectNotFoundException) {
                return false;
            }

            return true;
        }
    }
}
