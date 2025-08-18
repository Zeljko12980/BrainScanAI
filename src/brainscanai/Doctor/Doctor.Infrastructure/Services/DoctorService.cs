using Doctor.Application.Common.Interface;
using Doctor.Application.Dtos;
using Google.Protobuf;
using Grpc.Net.Client;
using Brain;



namespace Doctor.Infrastructure.Services
{
    public class DoctorService
        (DoctorDbContext context, IMapper mapper, string grpcAddress)
        : IDoctorService,IBrainTumorAnalyzer
    {
        public async Task<(string TumorType, double Confidence)> AnalyzeAsync(string imageBytes)
        {
            using var channel = GrpcChannel.ForAddress(grpcAddress);
            var client = new BrainTumorAnalyzer.BrainTumorAnalyzerClient(channel);

            var request = new TumorImageRequest
            {
                Image = imageBytes
            };

            var response = await client.PredictAsync(request);

            return (response.TumorType, response.Confidence);
        }

        public async Task<DoctorDto> CreateAsync(CreateDoctorDto dto)
        {
            var doctor=mapper.Map<Doctor.Domain.Entities.Doctor>(dto);

            context.Doctors.Add(doctor);
            await context.SaveChangesAsync();

            return mapper.Map<DoctorDto>(doctor);
        }

        public Task<bool> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DoctorDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<DoctorDto?> GetByIdAsync(Guid id)
        {
           var doctor = await context.Doctors.FirstOrDefaultAsync(x=>x.Id==id);

            return mapper.Map<DoctorDto>(doctor);
        }

        public Task<DoctorDto?> UpdateAsync(Guid id, CreateDoctorDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
