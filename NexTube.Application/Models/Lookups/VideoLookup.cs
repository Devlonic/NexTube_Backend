namespace NexTube.Application.Models.Lookups {
    public record VideoLookup {
        public int? Id { get; set; } = null;
        public string? Name { get; set; } = null;
        public string? Description { get; set; } = null;
        public string? AccessModificator { get; set; } = null;
        public Guid? VideoFile { get; set; } = null;
        public Guid? PreviewPhotoFile { get; set; } = null;
        public UserLookup? Creator { get; set; } = null;
        public DateTime? DateCreated { get; set; } = null;
        public DateTime? DateModified { get; set; } = null;
        public int? Views { get; set; } = null;
        public int? CommentsCount { get; set; } = null;
        public bool? IsApproved { get; set; }
    }
}
