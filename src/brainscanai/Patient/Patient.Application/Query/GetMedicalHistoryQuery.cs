
using Patient.Application.Common.Interface;
using Patient.Application.Dtos;

namespace Patient.Application.Query
{
    public class MedicalHistoryQuery:IQuery<MedicalHistoryDto>
    {
        public Guid Id { get; set; }    
    }

    public class MedicalHistoryQueryHandler
        (IPatientService patientService)
        : IQueryHandler<MedicalHistoryQuery, MedicalHistoryDto>
    {
        public async Task<MedicalHistoryDto> Handle(MedicalHistoryQuery query, CancellationToken cancellationToken)
        {
            var medicalHistory = await patientService.GetMedicalHistoryAsync(query.Id);

            return medicalHistory;
        }
    }
}
