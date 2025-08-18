using Patient.Application.Common.Interface;
using Patient.Application.Dtos;

namespace Patient.Application.Query
{
    public class GetAllPatientsQuery:IQuery<IEnumerable<PatientDto>>
    {
    }

    public class GetAllPatientsQueryHandler
        (IPatientService patientService)
        : IQueryHandler<GetAllPatientsQuery, IEnumerable<PatientDto>>
    {
        public async Task<IEnumerable<PatientDto>> Handle(GetAllPatientsQuery query, CancellationToken cancellationToken)
        {
            var patients = await patientService.GetAllAsync();

            return patients;
        }
    }
}
