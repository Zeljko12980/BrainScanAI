using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Patient.Domain.ValueObjects;

namespace Patient.Infrastructure.Persistence
{
    public class PatientDbContext : DbContext
    {
        public PatientDbContext(DbContextOptions<PatientDbContext> options)
            : base(options)
        {
        }

        public DbSet<Patient.Domain.Entities.Patient> Patients => Set<Patient.Domain.Entities.Patient>();
        public DbSet<Allergy> Allergies => Set<Allergy>();
        public DbSet<ChronicDisease> ChronicDiseases => Set<ChronicDisease>();
        public DbSet<Medication> Medications => Set<Medication>();
        public DbSet<ScanImage> ScanImages => Set<ScanImage>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

           
            modelBuilder.Entity<Patient.Domain.Entities.Patient>()
                .OwnsOne(p => p.FullName);

            modelBuilder.Entity<Patient.Domain.Entities.Patient>()
                .OwnsOne(p => p.Jmbg);

            modelBuilder.Entity<Patient.Domain.Entities.Patient>()
                .OwnsOne(p => p.Contact);

            var bloodTypeConverter = new ValueConverter<BloodType, string>(
           v => v.Type,
           v => new BloodType(v));

            modelBuilder.Entity<Patient.Domain.Entities.Patient>()
                .Property(p => p.BloodType)
                .HasConversion(bloodTypeConverter);

            modelBuilder.Entity<Patient.Domain.Entities.Patient>()
                .OwnsOne(p => p.EmergencyContact);

            modelBuilder.Entity<Patient.Domain.Entities.Patient>()
     .OwnsOne(p => p.MedicalHistory, mh =>
     {
         mh.OwnsMany(m => m.Events, me =>
         {
             me.WithOwner().HasForeignKey("MedicalHistoryId");
             me.Property<Guid>("Id");
             me.HasKey("Id");
             me.ToTable("MedicalEvents");
         });
     });
            modelBuilder.Entity<Patient.Domain.Entities.Patient>().HasQueryFilter(p => !p.IsDeleted);
        }
    }
}
