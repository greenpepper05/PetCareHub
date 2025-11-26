using System.Linq.Expressions;
using Core.Entities;

namespace Core.Specifications;

public class PetsByClinicIdSpecification : BaseSpecification<Pet>
{
    public PetsByClinicIdSpecification(PetSpecParams specParams, int clinicId) : base(p => p.Appointments.Any(a => a.ClinicId == clinicId && (string.IsNullOrEmpty(specParams.Search) || p.Name.Contains(specParams.Search) || p.Species.Contains(specParams.Search))))
    {
        AddInclude(p => p.Owner);
        ApplyPaging(specParams.PageSize * (specParams.PageIndex - 1), specParams.PageSize);

    }

}
