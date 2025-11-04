using System.Linq.Expressions;
using Core.Entities;

namespace Core.Specifications;

public class PetsByClinicIdSpecification : BaseSpecification<Appointment>
{
    public PetsByClinicIdSpecification(PetSpecParams specParams, int clinicId) : base(p => p.ClinicId == clinicId)
    {
        ApplyPaging(specParams.PageSize * (specParams.PageIndex - 1), specParams.PageSize);

        AddInclude(sr => sr.Pet!);
    }

    public PetsByClinicIdSpecification(int clinicId) : base(p => p.ClinicId == clinicId)
    {
        AddInclude(sr => sr.Pet!);
    }
}
