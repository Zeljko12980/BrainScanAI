
using Doctor.Application.Dtos;
using Doctor.Domain.ValueObjects;

namespace Doctor.Infrastructure.MappingProfle
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateDoctorDto, Doctor.Domain.Entities.Doctor>()
               .ForMember(dest => dest.Id, opt => opt.Ignore())
               .ForMember(dest => dest.Name, opt => opt.MapFrom(src => new FullName(src.FirstName, src.LastName)))
               .ForMember(dest => dest.Email, opt => opt.MapFrom(src => new Email(src.Email)))
               .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => new PhoneNumber(src.PhoneNumber)))
               .ForMember(dest => dest.Specialization, opt => opt.MapFrom(src => src.Specialization))
               .ForMember(dest => dest.License, opt => opt.MapFrom(src=>src.License))
               .ForMember(dest => dest.Jmbg, opt => opt.MapFrom(src=>new Jmbg(src.Jmbg)))
               .ForMember(dest => dest.PatientIds, opt => opt.MapFrom(_ => new List<Guid>()))
               .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));

            CreateMap<Doctor.Domain.Entities.Doctor, DoctorDto>()
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
               .ForMember(dest => dest.Firstname, opt => opt.MapFrom(src => src.Name.FirstName))
               .ForMember(dest => dest.Lastname, opt => opt.MapFrom(src => src.Name.LastName))
               .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.Address))
               .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.Phone.Number))
               .ForMember(dest => dest.Specialization, opt => opt.MapFrom(src => src.Specialization));
        }
    }
}
