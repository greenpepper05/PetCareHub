using System;
using System.Linq.Expressions;
using Core.Entities;

namespace Core.Specifications;

public class ServiceRecordByPetIdSpecification : BaseSpecification<ServiceRecord>
{
    public ServiceRecordByPetIdSpecification(int id) : base(sr => sr.PetId == id)
    {
        AddInclude(s => s.Pet!);
        AddInclude(s => s.Service!);
        AddInclude(s => s.Clinic!);
    }

    protected ServiceRecordByPetIdSpecification()
    {
    }
}
