using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patient.Application.Dtos
{
    public class UpdatePatientDto
    {
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string Jmbg { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Address { get; set; } = default!;
        public string EmergencyContactName { get; set; } = default!;
        public string EmergencyContactPhone { get; set; } = default!;
        public string BloodType { get; set; } = default!;
    }
}
