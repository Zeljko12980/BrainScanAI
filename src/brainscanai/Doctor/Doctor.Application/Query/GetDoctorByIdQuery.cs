using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctor.Application.Query
{
    public class GetDoctorByIdQuery:IQuery<DoctorDto>
    {
        public Guid Id { get; set; }
    }

    public class GetDoctorByIdQueryHandler
        (IDoctorService doctorService)
        : IQueryHandler<GetDoctorByIdQuery, DoctorDto>
    {
        public async Task<DoctorDto> Handle(GetDoctorByIdQuery query, CancellationToken cancellationToken)
        {
            var result = await doctorService.GetByIdAsync(query.Id);

            return result;
        }
    }
}
