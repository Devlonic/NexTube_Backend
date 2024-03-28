using NexTube.Application.Models.Lookups;
using NexTube.Domain.Entities;

namespace NexTube.Application.Models {
    public class VideoUploadProgress : FileUploadProgress {
        public VideoLookup? Video { get; set; }
    }
}
