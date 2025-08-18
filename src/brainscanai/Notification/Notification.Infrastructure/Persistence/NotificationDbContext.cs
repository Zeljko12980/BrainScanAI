namespace Notification.Infrastructure.Persistence
{
    public class NotificationDbContext:DbContext
    {
        public NotificationDbContext(DbContextOptions<NotificationDbContext> options):base(options)
        {
            
        }

        public DbSet<Notification.Domain.Entities.Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Notification.Domain.Entities.Notification>(entity =>
            {
                entity.HasKey(n => n.Id);
                entity.Property(n => n.Title).HasMaxLength(100).IsRequired();
                entity.Property(n => n.Message).HasMaxLength(500).IsRequired();
                entity.Property(n => n.Icon).HasMaxLength(30).IsRequired();
            });
        }
    }
}
