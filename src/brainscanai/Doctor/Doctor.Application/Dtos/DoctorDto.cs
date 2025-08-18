namespace Doctor.Application.Dtos
{
    public class DoctorDto
    {
        public Guid Id { get; set; }
        public string Firstname { get; set; } = default!;
        public string Lastname { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
        public string Specialization { get; set; } = default!;
    }
}
