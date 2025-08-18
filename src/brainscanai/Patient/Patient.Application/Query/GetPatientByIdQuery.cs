

using Patient.Application.Common.Interface;
using Patient.Application.Dtos;

namespace Patient.Application.Query
{
    public class GetPatientByIdQuery:IQuery<PatientDto>
    {
        public Guid Id { get; set; }
    }

    public class GetPatientByIdHandler 
        (IPatientService patientService)
        : IQueryHandler<GetPatientByIdQuery, PatientDto>
    {
        public async Task<PatientDto> Handle(GetPatientByIdQuery query, CancellationToken cancellationToken)
        {
            var patient = await patientService.GetByIdAsync(query.Id);

            return patient;
        }
    }
}
