using System;
using System.Linq.Expressions;
using Core.Entities;

namespace Core.Specifications;

public class ServiceRecordByPetIdAndClinicIdSpecification : BaseSpecification<ServiceRecord>
{
    public ServiceRecordByPetIdAndClinicIdSpecification(ServiceRecordSpecParams specParams, int petId, int clinicId) : base(sr => sr.PetId == petId && sr.ClinicId == clinicId)
    {
        ApplyPaging(specParams.PageSize * (specParams.PageIndex - 1), specParams.PageSize);

        AddInclude(sr => sr.Pet!);
        AddInclude(sr => sr.Pet!.Owner);
        AddInclude(sr => sr.Clinic!);
        AddInclude(sr => sr.Service!);
        AddOrderByDescending(sr => sr.DateOfService);
    }

    public ServiceRecordByPetIdAndClinicIdSpecification()
    {
    }
}
