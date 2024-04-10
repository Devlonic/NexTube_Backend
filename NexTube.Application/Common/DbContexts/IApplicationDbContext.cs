using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NexTube.Domain.Entities;
using NexTube.Domain.Entities.ManyToMany;

namespace NexTube.Application.Common.DbContexts {
    public interface IApplicationDbContext {
        DbSet<VideoEntity> Videos { get; set; }
        DbSet<VideoCommentEntity> VideoComments { get; set; }
        DbSet<VideoAccessModificatorEntity> VideoAccessModificators { get; set; }
        DbSet<VideoReactionEntity> VideoReactions { get; set; }
        DbSet<VideoPlaylistEntity> VideoPlaylists { get; set; }
        DbSet<PlaylistsVideosManyToMany> PlaylistsVideosManyToMany { get; set; }
        DbSet<SubscriptionEntity> Subscriptions { get; set; }
        DbSet<Report> Reports { get; set; }
        DbSet<NotificationEntity> Notifications { get; set; }
        DbSet<UserVideoHistoryEntity> UserVideoHistories { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
        IQueryable<VideoEntity> FromSqlVideos(FormattableString formattableString);
    }
}
