using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Patient.Domain.Entities;

namespace Patient.Infrastructure.Services
{
    public class PatientService 
        (PatientDbContext context,IMapper mapper)
        : IPatientService
    {
        public async Task<bool> AddAllergyAsync(Guid patientId, CreateAllergyDto newAllergy)
        {
            try
            {
                var patient = await context.Patients
                    .Include(p => p.Allergies)
                    .FirstOrDefaultAsync(p => p.Id == patientId);

                if (patient == null)
                    return false;

                var allergy = Allergy.Create(patientId, newAllergy.Name, newAllergy.Severity, newAllergy.Notes);

                context.Set<Allergy>().Add(allergy);

                var saved = await context.SaveChangesAsync() > 0;
                if (!saved)
                    throw new InvalidOperationException("Failed to save allergy.");

                return true;
            }
            catch (Exception ex)
            {
              

                throw new ApplicationException($"Error adding allergy for patient {patientId}", ex);
            }
        }

        public async Task<bool> AddChronicDiseaseAsync(Guid patientId, CreateChronicDiseaseDto newDisease)
        {
            try
            {
                var patient = await context.Patients
                    .Include(p => p.ChronicDiseases)
                    .FirstOrDefaultAsync(p => p.Id == patientId);

                if (patient == null)
                    return false;

                var chronicDisease = ChronicDisease.Create(
             patientId,
             newDisease.Name,
             newDisease.Severity,
             newDisease.Description
         );

                context.Set<ChronicDisease>().Add(chronicDisease);

                var saved = await context.SaveChangesAsync() > 0;
                if (!saved)
                    throw new InvalidOperationException("Failed to save chronic disease.");

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> AddMedicationAsync(Guid patientId, CreateMedicationDto newMedication)
        {
            try
            {
                var patient = await context.Patients
                    .Include(p => p.Medications)
                    .FirstOrDefaultAsync(p => p.Id == patientId);

                if (patient == null)
                    return false;

                var medication = Medication.Create(
           patientId,
           newMedication.Name,
           newMedication.Dosage
       );

                context.Set<Medication>().Add(medication);

                var saved = await context.SaveChangesAsync() > 0;
                if (!saved)
                    throw new InvalidOperationException("Failed to save medication.");

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> AddScanImageAsync(Guid patientId, CreateScanImageDto newImage)
        {
            try
            {
                var patient = await context.Patients
                    .Include(p => p.ScanImages)
                    .FirstOrDefaultAsync(p => p.Id == patientId);

                if (patient == null)
                    return false;

                var scanImage = ScanImage.Create(
              patientId,
              newImage.ImageType,
              newImage.Url,
              newImage.TakenAt
          );

                context.Set<ScanImage>().Add(scanImage);

                var saved = await context.SaveChangesAsync() > 0;
                if (!saved)
                    throw new InvalidOperationException("Failed to save scan image.");

                return true;
            }
            catch
            {
                return false;
            }
        }


        public async Task<bool> AddMedicalEventAsync(Guid patientId, MedicalEventDto newEvent)
        {
            try
            {
                var patient = await context.Patients
                    .Include(p => p.MedicalHistory) 
                    .FirstOrDefaultAsync(p => p.Id == patientId);

                if (patient == null)
                    return false;

                var medicalEvent = mapper.Map<Patient.Domain.ValueObjects.MedicalHistory.MedicalEvent>(newEvent);

                
                patient.MedicalHistory.AddEvent(medicalEvent);

                context.Patients.Update(patient);
                var saved = await context.SaveChangesAsync() > 0;

                return saved;
            }
            catch (Exception ex)
            {
               
                return false;
            }
        }

     

        public async Task<PatientDto> CreateAsync(CreatePatientDto patientDto, Guid doctorId)
        {
            try
            {
                var patient = mapper.Map<Patient.Domain.Entities.Patient>(patientDto);
                patient.DoctorId = doctorId;
                context.Patients.Add(patient);
              
                await context.SaveChangesAsync();
                return mapper.Map<PatientDto>(patient);
            }
            catch (Exception ex)
            {
                
                throw new ApplicationException("Error creating patient", ex);
            }
        }


        public async Task<bool> DeleteAsync(Guid id)
        {
            var patient = await context.Patients.FirstOrDefaultAsync(x => x.Id == id);

            if (patient == null)
                return false;

            patient.IsDeleted = true;
            patient.UpdatedAt = DateTime.UtcNow;

            await context.SaveChangesAsync();
            return true;
        }


        public async Task<bool> DeleteMedicalEventAsync(Guid patientId, Guid eventId)
        {
            var patient = await context.Patients
                .FirstOrDefaultAsync(p => p.Id == patientId);

            if (patient == null)
                return false;

            var medicalEvent = patient.MedicalHistory.Events
                .FirstOrDefault(e => e.Id == eventId);

            if (medicalEvent == null)
                return false;

            patient.MedicalHistory.Events.Remove(medicalEvent);

            context.Patients.Update(patient);
            var saved = await context.SaveChangesAsync() > 0;

            return saved;
        }


        public async Task<IEnumerable<PatientDto>> GetAllAsync()
        {
            var patients = await context.Patients.ToListAsync();
            var patientDtos = mapper.Map<IEnumerable<PatientDto>>(patients);
            return patientDtos;
        }

        public async Task<PatientDto?> GetByIdAsync(Guid id)
        {
            var patient = await context.Patients
        .Include(p => p.Allergies)
        .Include(p => p.ChronicDiseases)
        .Include(p => p.Medications)
        .Include(p => p.ScanImages)
        .FirstOrDefaultAsync(p => p.Id == id);

            if (patient == null)
                return null;

            return mapper.Map<PatientDto>(patient);
        }

        public async Task<MedicalHistoryDto?> GetMedicalHistoryAsync(Guid patientId)
        {
            var patient = await context.Patients
                .Include(x => x.MedicalHistory)
                .ThenInclude(mh => mh.Events)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == patientId);

            if (patient == null) return null;

            return mapper.Map<MedicalHistoryDto>(patient.MedicalHistory);
        }


        public async Task<bool> UpdateAsync(Guid id, UpdatePatientDto updatedPatientDto)
        {
            var existingPatient = await context.Patients.FindAsync(id);
            if (existingPatient is null)
                return false;

            mapper.Map(updatedPatientDto, existingPatient);
            existingPatient.UpdatedAt = DateTime.UtcNow;

            context.Patients.Update(existingPatient);
            await context.SaveChangesAsync();

            return true;
        }


        public async Task<bool> UpdateMedicalEventAsync(Guid patientId, Guid eventId, MedicalEventDto updatedEvent)
        {
            var patient = await context.Patients
                .Include(x=>x.MedicalHistory)
                .ThenInclude(p => p.Events)
                .FirstOrDefaultAsync(p => p.Id == patientId);

            if (patient == null)
                return false;

            var existingEvent = patient.MedicalHistory.Events.FirstOrDefault(me => me.Id == eventId);
            if (existingEvent == null)
                return false;


            existingEvent.Description = updatedEvent.Description;
            existingEvent.Date = updatedEvent.Date;
            existingEvent.EventType = updatedEvent.Type;

            context.Patients.Update(patient);
            await context.SaveChangesAsync();

            return true;
        }

    }
}
