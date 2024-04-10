using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NexTube.Application.Common.DbContexts;
using NexTube.Domain.Entities;
using NexTube.Domain.Entities.ManyToMany;
using NexTube.Persistence.Data.Configurations.Comments.VideoComments;
using NexTube.Persistence.Data.Configurations.History;
using NexTube.Persistence.Data.Configurations.Identity;
using NexTube.Persistence.Data.Configurations.Notifications;
using NexTube.Persistence.Data.Configurations.Playlists;
using NexTube.Persistence.Data.Configurations.Reactions;
using NexTube.Persistence.Data.Configurations.Videos;

namespace NexTube.Persistence.Data.Contexts {
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int>, IApplicationDbContext {
        public DbSet<VideoEntity> Videos { get; set; } = null!;
        public DbSet<VideoCommentEntity> VideoComments { get; set; } = null!;
        public DbSet<VideoReactionEntity> VideoReactions { get; set; } = null!;
        public DbSet<VideoAccessModificatorEntity> VideoAccessModificators { get; set; } = null!;
        public DbSet<VideoPlaylistEntity> VideoPlaylists { get; set; } = null!;
        public DbSet<PlaylistsVideosManyToMany> PlaylistsVideosManyToMany { get; set; } = null!;
        public DbSet<SubscriptionEntity> Subscriptions { get; set; } = null!;
        public DbSet<Report> Reports { get; set; } = null!;
        public DbSet<NotificationEntity> Notifications { get; set; } = null!;
        public DbSet<UserVideoHistoryEntity> UserVideoHistories { get; set; } = null!;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder builder) {
            builder.ApplyConfiguration(new ApplicationUserConfiguration());
            builder.ApplyConfiguration(new ApplicationRoleConfiguration());
            builder.ApplyConfiguration(new ReportConfiguration());
            builder.ApplyConfiguration(new VideoEntityConfiguration());
            builder.ApplyConfiguration(new VideoAccessModificatorEntityConfiguration());
            builder.ApplyConfiguration(new VideoCommentEntityConfiguration());
            builder.ApplyConfiguration(new VideoReactionEntityConfiguration());
            builder.ApplyConfiguration(new SubscriptionEntityConfiguration());
            builder.ApplyConfiguration(new VideoPlaylistEntityConfiguration());
            builder.ApplyConfiguration(new PlaylistsVideosManyToManyConfiguration());
            builder.ApplyConfiguration(new NotificationEntityConfiguration());
            builder.ApplyConfiguration(new UserVideoHistoryConfiguration());

            base.OnModelCreating(builder);
        }
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) {
            // default behaviour
            return await base.SaveChangesAsync(cancellationToken);
        }

        public IQueryable<VideoEntity> FromSqlVideos(FormattableString formattableString) {
            return Videos.FromSql(formattableString);
        }
    }
}
