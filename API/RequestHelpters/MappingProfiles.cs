using API.DTOs;
using AutoMapper;
using Core.Entities;

namespace API.RequestHelpters;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Appointment, AppointmentDto>()
            .ForMember(d => d.ServiceId, o => o.MapFrom(s => s.ServiceId))
            .ForMember(d => d.PetName, o => o.MapFrom(s => s.Pet!.Name))
            .ForMember(d => d.PetBreed, o => o.MapFrom(s => s.Pet!.Breed))
            .ForMember(d => d.PetSpecies, o => o.MapFrom(s => s.Pet!.Species))
            .ForMember(d => d.PetGender, o => o.MapFrom(s => s.Pet!.Gender))
            .ForMember(d => d.PetBirthdate, o => o.MapFrom(s => s.Pet!.Birthdate))
            .ForMember(d => d.ServiceName, o => o.MapFrom(s => s.Service!.Name))
            .ForMember(d => d.ClinicName, o => o.MapFrom(s => s.Clinic!.ClinicName))
            .ForMember(d => d.OwnerEmail, o => o.MapFrom(s => s.Owner!.Email));

        CreateMap<CreatePetServiceHistoryDto, PetServiceHistory>();
        CreateMap<PetServiceHistory, PetServiceHistoryDto>()
            .ForMember(d => d.PetName, o => o.MapFrom(s => s.Pet!.Name))
            .ForMember(d => d.PetId, o => o.MapFrom(s => s.Pet!.Id))
            .ForMember(d => d.ServiceName, o => o.MapFrom(s => s.Service!.Name))
            .ForMember(d => d.ClinicName, o => o.MapFrom(s => s.Clinic!.ClinicName))
            .ForMember(d => d.DateOfService, o => o.MapFrom(s => s.DateOfService))
            .ForMember(d => d.VisitType, o => o.MapFrom(s => s.VisitType.ToString()));

        CreateMap<CreateServiceRecordDto, ServiceRecord>();
        CreateMap<ServiceRecord, ServiceRecordDto>()
            .ForMember(d => d.PetName, o => o.MapFrom(s => s.Pet!.Name))
            .ForMember(d => d.PetId, o => o.MapFrom(s => s.Pet!.Id))
            .ForMember(d => d.ServiceName, o => o.MapFrom(s => s.Service!.Name))
            .ForMember(d => d.ClinicName, o => o.MapFrom(s => s.Clinic!.ClinicName))
            .ForMember(d => d.DateOfService, o => o.MapFrom(s => s.DateOfService))
            .ForMember(d => d.VisitType, o => o.MapFrom(s => s.VisitType.ToString()));
    }
}
