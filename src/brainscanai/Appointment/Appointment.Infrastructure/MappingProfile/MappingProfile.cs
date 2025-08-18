namespace Appointment.Infrastructure.MappingProfile
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<Appointment.Domain.Entities.Appointment, AppointmentDto>()
               .ForMember(dest => dest.DoctorName, opt => opt.MapFrom(src => src.DoctorName))
               .ForMember(dest => dest.DoctorSpecialty, opt => opt.MapFrom(src => src.Specialty))
               .ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => src.PatientName));
        }
    }
}
