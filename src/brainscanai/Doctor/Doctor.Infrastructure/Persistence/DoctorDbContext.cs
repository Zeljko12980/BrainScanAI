namespace Doctor.Infrastructure.Persistence
{
    public class DoctorDbContext:DbContext
    {

        public DoctorDbContext(DbContextOptions<DoctorDbContext> options)
           : base(options)
        {
        }

        public DbSet<Doctor.Domain.Entities.Doctor> Doctors => Set<Doctor.Domain.Entities.Doctor>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Doctor.Domain.Entities.Doctor>(entity =>
            {
                entity.HasKey(d => d.Id);

                entity.Property(d => d.Specialization)
                      .HasMaxLength(100)
                      .IsRequired();

               
                entity.OwnsOne(d => d.Name, name =>
                {
                    name.Property(n => n.FirstName)
                        .HasColumnName("FirstName")
                        .HasMaxLength(100)
                        .IsRequired();

                    name.Property(n => n.LastName)
                        .HasColumnName("LastName")
                        .HasMaxLength(100)
                        .IsRequired();
                });

               
                entity.OwnsOne(d => d.Email, email =>
                {
                    email.Property(e => e.Address)
                         .HasColumnName("Email")
                         .HasMaxLength(100)
                         .IsRequired();
                });


                entity.OwnsOne(d => d.Phone, phone =>
                {
                    phone.Property(p => p.Number)
                         .HasColumnName("Phone")
                         .HasMaxLength(100)
                         .IsRequired();
                });

             
                entity.OwnsOne(d => d.License, license =>
                {
                    license.Property(l => l.Number)
                           .HasColumnName("License")
                           .HasMaxLength(100)
                           .IsRequired();
                });

               
                entity.OwnsOne(d => d.Jmbg, jmbg =>
                {
                    jmbg.Property(j => j.Value)
                        .HasColumnName("Jmbg")
                        .HasMaxLength(13)
                        .IsRequired();
                });

              
                entity.Property(d => d.PatientIds)
                      .HasConversion(
                          v => string.Join(',', v),
                          v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(Guid.Parse).ToList())
                      .HasColumnName("PatientIds");

                entity.Property(d => d.CreatedAt)
                      .IsRequired();
            });
        }

    }
}
