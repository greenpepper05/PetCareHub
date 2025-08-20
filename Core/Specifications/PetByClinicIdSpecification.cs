using System;
using System.Linq.Expressions;
using Core.Entities;

namespace Core.Specifications;

public class PetByClinicIdSpecification : BaseSpecification<Pet>
{
    // public PetByClinicIdSpecification(int clinicId) : base(p => p.ClinicId == clinicId)
    // {
    //     AddInclude(x => x.Clinic!);
    // }
    
}
