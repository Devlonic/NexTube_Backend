using NexTube.Domain.Entities.Abstract;
using NexTube.Domain.Entities.ManyToMany;

namespace NexTube.Domain.Entities {
    public class VideoEntity : OwnedEntity {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public Guid? VideoFileId { get; set; } = null!;
        public Guid? PreviewPhotoFileId { get; set; } = null!;
        public int Views { get; set; } = 0;
        public bool IsApproved { get; set; }

        public VideoAccessModificatorEntity? AccessModificator { get; set; } = null!;

        public virtual ICollection<PlaylistsVideosManyToMany> PlaylistsVideos { get; set; } = null!;
    }
}
