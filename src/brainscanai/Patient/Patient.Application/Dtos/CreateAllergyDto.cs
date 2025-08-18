global using Patient.Domain.Enums;

namespace Patient.Application.Dtos
{
    public class CreateAllergyDto
    {
        public string Name { get; set; } = null!;
        public Severity Severity { get; set; }
        public string? Notes { get; set; }
    }
}
