using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patient.Application.Dtos
{
    public class CreateMedicationDto
    {
        public string Name { get; set; } = null!;
        public string Dosage { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
