using Patient.Application.Dtos;
namespace Patient.Application.Common.Interface
{
    public interface IPatientService
    {
        Task<PatientDto> CreateAsync(CreatePatientDto patientDto,Guid doctorId);
        Task<bool> UpdateAsync(Guid id, UpdatePatientDto updatedPatientDto);
        Task<bool> DeleteAsync(Guid id);

        Task<bool> AddMedicalEventAsync(Guid patientId, MedicalEventDto newEvent);
        Task<bool> UpdateMedicalEventAsync(Guid patientId, Guid eventId, MedicalEventDto updatedEvent);
        Task<bool> DeleteMedicalEventAsync(Guid patientId, Guid eventId);

        Task<IEnumerable<PatientDto>> GetAllAsync();
        Task<PatientDto?> GetByIdAsync(Guid id);
        Task<MedicalHistoryDto?> GetMedicalHistoryAsync(Guid patientId);
        Task<bool> AddScanImageAsync(Guid patientId, CreateScanImageDto newImage);
        Task<bool> AddAllergyAsync(Guid patientId, CreateAllergyDto newAllergy);
        Task<bool> AddChronicDiseaseAsync(Guid patientId, CreateChronicDiseaseDto newDisease);
        Task<bool> AddMedicationAsync(Guid patientId, CreateMedicationDto newMedication);


    }
}
