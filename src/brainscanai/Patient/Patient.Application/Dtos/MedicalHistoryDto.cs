using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patient.Application.Dtos
{
    public class MedicalHistoryDto
    {
        public List<MedicalEventDto> Events { get; set; } = new();
    }
}
