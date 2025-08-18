using Patient.Domain.ValueObjects;
using static Patient.Domain.ValueObjects.MedicalHistory;

namespace Patient.Infrastructure.MappingProfile
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Patient.Domain.Entities.Patient, PatientDto>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FullName.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.FullName.LastName))
                .ForMember(dest => dest.Jmbg, opt => opt.MapFrom(src => src.Jmbg.Value))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Contact.PhoneNumber))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Contact.Email))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Contact.Address))
                .ForMember(dest => dest.BloodType, opt => opt.MapFrom(src => src.BloodType.Type))
                .ForMember(dest => dest.EmergencyContactName, opt => opt.MapFrom(src => src.EmergencyContact.Name))
                .ForMember(dest => dest.EmergencyContactPhone, opt => opt.MapFrom(src => src.EmergencyContact.Phone))
                .ForMember(dest => dest.EmergencyContactRelation, opt => opt.MapFrom(src => src.EmergencyContact.Name))
                .ForMember(dest => dest.Allergies, opt => opt.MapFrom(src => src.Allergies.Select(a => a.Name)))
                .ForMember(dest => dest.ChronicDiseases, opt => opt.MapFrom(src => src.ChronicDiseases.Select(d => d.Name)))
                .ForMember(dest => dest.Medications, opt => opt.MapFrom(src => src.Medications.Select(m => m.Name)))
                 .ForMember(dest => dest.MedicalHistory, opt => opt.MapFrom(src => src.MedicalHistory))
                 .ForMember(dest => dest.ScanImages, opt => opt.MapFrom(src => src.ScanImages.Select(m=>m.Url)));
                 ; 

            CreateMap<CreatePatientDto, Patient.Domain.Entities.Patient>()
         .ForMember(dest => dest.FullName,
             opt => opt.MapFrom(src => new Fullname(src.FirstName, src.LastName)))
         .ForMember(dest => dest.Jmbg,
             opt => opt.MapFrom(src => new Jmbg(src.Jmbg)))
         .ForMember(dest => dest.Contact,
             opt => opt.MapFrom(src => new ContactInformation(src.PhoneNumber, src.Email, src.Address)))
         .ForMember(dest => dest.EmergencyContact,
             opt => opt.MapFrom(src => new EmergencyContact(src.EmergencyContactName, src.EmergencyContactPhone)))
         .ForMember(dest => dest.BloodType,
             opt => opt.MapFrom(src => new BloodType(src.BloodType)))
         .ForMember(dest => dest.MedicalHistory,
             opt => opt.MapFrom(_ => new MedicalHistory()))
         .ForMember(dest => dest.Allergies, opt => opt.Ignore())
         .ForMember(dest => dest.ChronicDiseases, opt => opt.Ignore())
         .ForMember(dest => dest.Medications, opt => opt.Ignore())
         .ForMember(dest => dest.ScanImages, opt => opt.Ignore())
         .ForMember(dest => dest.Id, opt => opt.Ignore())
         .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
         .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

            CreateMap<MedicalEventDto, MedicalEvent>()
                    .ConstructUsing(dto => new MedicalEvent(dto.Date, dto.Type, dto.Description));
            CreateMap<MedicalHistory, MedicalHistoryDto>()
           .ForMember(dest => dest.Events, opt => opt.MapFrom(src => src.Events));


            CreateMap<MedicalHistoryDto, MedicalHistory>();

            CreateMap<MedicalHistory.MedicalEvent, MedicalEventDto>()
          .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.EventType))
          .ReverseMap()
          .ForCtorParam("eventType", opt => opt.MapFrom(src => src.Type));


            CreateMap<UpdatePatientDto, Patient.Domain.Entities.Patient>()
    .ForMember(dest => dest.FullName,
        opt => opt.MapFrom(src => new Fullname(src.FirstName, src.LastName)))
    .ForMember(dest => dest.Jmbg,
        opt => opt.MapFrom(src => new Jmbg(src.Jmbg)))
    .ForMember(dest => dest.Contact,
        opt => opt.MapFrom(src => new ContactInformation(src.PhoneNumber, src.Email, src.Address)))
    .ForMember(dest => dest.EmergencyContact,
        opt => opt.MapFrom(src => new EmergencyContact(src.EmergencyContactName, src.EmergencyContactPhone)))
    .ForMember(dest => dest.BloodType,
        opt => opt.MapFrom(src => new BloodType(src.BloodType)))
    .ForMember(dest => dest.Id, opt => opt.Ignore())
    .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
    .ForMember(dest => dest.MedicalHistory, opt => opt.Ignore())
    .ForMember(dest => dest.Allergies, opt => opt.Ignore())
    .ForMember(dest => dest.ChronicDiseases, opt => opt.Ignore())
    .ForMember(dest => dest.Medications, opt => opt.Ignore())
    .ForMember(dest => dest.ScanImages, opt => opt.Ignore());

        }
    }
}
