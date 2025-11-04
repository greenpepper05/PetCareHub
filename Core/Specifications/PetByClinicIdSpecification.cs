using System;
using System.Linq.Expressions;
using Core.Entities;

namespace Core.Specifications;

public class PetByClinicIdSpecification : BaseSpecification<Appointment>
{
    public PetByClinicIdSpecification(int clinicId) : base(p => p.ClinicId == clinicId)
    {
        AddInclude(sr => sr.Pet!);
    }
    
}
