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
            .ForMember(d => d.ServiceId, o => o.MapFrom(s => s.ServiceId))
            .ForMember(d => d.ClinicName, o => o.MapFrom(s => s.Clinic!.ClinicName))
            .ForMember(d => d.Price, o => o.MapFrom(s => s.Price))
            .ForMember(d => d.DateOfService, o => o.MapFrom(s => s.DateOfService))
            .ForMember(d => d.AppointmentId, o => o.MapFrom(s => s.AppointmentId))
            .ForMember(d => d.VisitType, o => o.MapFrom(s => s.VisitType.ToString()));

        CreateMap<Procedure, ProcedureDto>()
            .ForMember(p => p.ServiceId, o => o.MapFrom(r => r.ServiceId))
            .ForMember(p => p.Name, o => o.MapFrom(r => r.Name))
            .ForMember(p => p.Description, o => o.MapFrom(r => r.Description))
            .ForMember(p => p.Order, o => o.MapFrom(r => r.Order));

        CreateMap<ServiceRecordStep, ServiceRecordStepDto>()
            .ForMember(p => p.ProcedureId, o => o.MapFrom(r => r.ProcedureId))
            .ForMember(p => p.Name, o => o.MapFrom(r => r.Name))
            .ForMember(p => p.Description, o => o.MapFrom(r => r.Description))
            .ForMember(p => p.Order, o => o.MapFrom(r => r.Order))
            .ForMember(p => p.IsCompleted, o => o.MapFrom(r => r.IsCompleted))
            .ForMember(p => p.IsSkipped, o => o.MapFrom(r => r.IsSkipped));

        CreateMap<Clinic, ClinicDto>()
            .ForMember(c => c.OwnerId, o => o.MapFrom(c => c.OwnerId))
            .ForMember(c => c.ClinicName, o => o.MapFrom(c => c.ClinicName))
            .ForMember(c => c.Address, o => o.MapFrom(c => c.Address))
            .ForMember(c => c.PhoneNumber, o => o.MapFrom(c => c.PhoneNumber))
            .ForMember(c => c.Email, o => o.MapFrom(c => c.Email))
            .ForMember(c => c.PictureUrl, o => o.MapFrom(c => c.PictureUrl))
            .ForMember(c => c.Status, o => o.MapFrom(c => c.Status))
            .ForMember(c => c.CreateAt, o => o.MapFrom(c => c.CreateAt))
            .ForMember(c => c.UpdatedAt, o => o.MapFrom(c => c.UpdatedAt));

        CreateMap<Pet, PetDto>()
            .ForMember(p => p.Name, o => o.MapFrom(p => p.Name))
            .ForMember(p => p.Birthdate, o => o.MapFrom(p => p.Birthdate))
            .ForMember(p => p.Breed, o => o.MapFrom(p => p.Breed))
            .ForMember(p => p.Species, o => o.MapFrom(p => p.Species))
            .ForMember(p => p.Gender, o => o.MapFrom(p => p.Gender))
            .ForMember(p => p.PictureUrl, o => o.MapFrom(p => p.PictureUrl))
            .ForMember(p => p.OwnerId, o => o.MapFrom(p => p.OwnerId));

        CreateMap<AppUser, UserDto>()
            .ForMember(u => u.Email, o => o.MapFrom(a => a.Email))
            .ForMember(u => u.FirstName, o => o.MapFrom(a => a.FirstName))
            .ForMember(u => u.LastName, o => o.MapFrom(a => a.LastName))
            .ForMember(u => u.Contact, o => o.MapFrom(a => a.Contact))
            .ForMember(u => u.ClinicId, o => o.MapFrom(a => a.ClinicId))
            .ForMember(u => u.ClinicName, o => o.MapFrom(a => a.Clinic!.ClinicName));

        CreateMap<Service, ServiceDto>();
    }
}