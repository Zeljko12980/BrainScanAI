namespace Doctor.Application.Common.Interface
{
    public interface IDoctorService
    {
        Task<DoctorDto> CreateAsync(CreateDoctorDto dto);
        Task<DoctorDto?> GetByIdAsync(Guid id);
        Task<IEnumerable<DoctorDto>> GetAllAsync();
        Task<bool> DeleteAsync(Guid id);
        Task<DoctorDto?> UpdateAsync(Guid id, CreateDoctorDto dto);
    }
}
