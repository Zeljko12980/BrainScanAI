using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patient.Application.Dtos
{
    public class CreateScanImageDto
    {
        public string ImageType { get; set; } = null!;
        public string Url { get; set; } = null!;
        public DateTime TakenAt { get; set; }
    }
}
