

using Doctor.Domain.ValueObjects;

namespace Doctor.Infrastructure.Persistence
{
    public static class DoctorSeed
    {
        public static async Task SeedAsync(DoctorDbContext context)
        {
            await context.Database.MigrateAsync();

            var doctorId = Guid.Parse("8d7c6b5f-09f4-4b22-a6a8-3b7dbe4f92f2"); 
            var patientId = Guid.Parse("c5a59d9e-02b4-4c72-a5e4-2c76b6d53133"); 

            if (!await context.Doctors.AnyAsync(d => d.Id == doctorId))
            {
                var doctor = new Doctor.Domain.Entities.Doctor
                {
                    Id = doctorId,
                    Name = new FullName("John", "Smith"),
                    Email = new Email("ikanoviczeljko095@gmail.com"),
                    Phone = new PhoneNumber("+123456789"),
                    License = new LicenseNumber("LIC-ONC-2025-001"),
                    Jmbg = new Jmbg("1709002173401"), 
                    Specialization = "Oncologist",
                    PatientIds = new List<Guid> { patientId }, 
                    CreatedAt = DateTime.UtcNow
                };

                await context.Doctors.AddAsync(doctor);
                await context.SaveChangesAsync();
            }
        }
    }
}
