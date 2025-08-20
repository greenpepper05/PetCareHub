using System;
using System.Linq.Expressions;
using Core.Entities;

namespace Core.Specifications;

public class PetServiceHistoryByClinicId : BaseSpecification<PetServiceHistory>
{
    public PetServiceHistoryByClinicId(int? clinicId, DateTime date) :
        base(p => p.ClinicId == clinicId && p.DateOfService.Date == date.Date)
    {
        AddInclude(x => x.Pet!);
        AddInclude(x => x.Service!);
        AddInclude(x => x.Clinic!);
        AddOrderByDescending(h => h.DateOfService);
    }

    protected PetServiceHistoryByClinicId()
    {
    }
}
