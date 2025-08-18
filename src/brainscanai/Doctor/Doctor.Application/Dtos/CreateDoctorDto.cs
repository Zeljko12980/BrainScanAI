

namespace Doctor.Application.Dtos
{
    public class CreateDoctorDto
    {
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
        public string Specialization { get; set; } = default;

  
        public string? License { get; set; }
        public string? Jmbg { get; set; }

      
        public List<Guid>? PatientIds { get; set; }
    }
}
