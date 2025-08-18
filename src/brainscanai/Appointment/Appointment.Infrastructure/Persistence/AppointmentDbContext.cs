namespace Appointment.Infrastructure.Persistence
{
    public class AppointmentDbContext : DbContext
    {
        public AppointmentDbContext(DbContextOptions<AppointmentDbContext> options) : base(options)
        {
        }

        public DbSet<Appointment.Domain.Entities.Appointment> Appointments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Appointment.Domain.Entities.Appointment>(entity =>
            {
               
                entity.HasKey(a => a.Id);

               
                entity.Property(a => a.Id)
                    .ValueGeneratedOnAdd();

                entity.Property(a => a.PatientId)
                    .IsRequired();

                entity.Property(a => a.DoctorId)
                    .IsRequired();

                entity.Property(a => a.AppointmentTime)
                    .IsRequired();

                entity.Property(a => a.Duration)
                    .IsRequired()
                    .HasDefaultValue(TimeSpan.FromMinutes(20))
                    .HasConversion(
                        v => v.TotalMinutes,
                        v => TimeSpan.FromMinutes(v));

                entity.Property(a => a.Location)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(a => a.Status)
                    .IsRequired()
                    .HasConversion<string>();

                entity.Property(a => a.Notes)
                    .HasMaxLength(1000);

                entity.Property(a => a.CreatedAt)
                    .IsRequired();

                entity.Property(a => a.UpdatedAt)
                    .IsRequired(false);

                entity.Property(a => a.DoctorName)
           .IsRequired()
           .HasMaxLength(100);

                entity.Property(a => a.Specialty)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(a => a.PatientName)
                    .IsRequired()
                    .HasMaxLength(100);


                entity.HasIndex(a => a.PatientId);
                entity.HasIndex(a => a.DoctorId);
                entity.HasIndex(a => a.AppointmentTime);
                entity.HasIndex(a => a.Status);

                
                entity.ToTable("Appointments");

               
            });
        }
    }
}