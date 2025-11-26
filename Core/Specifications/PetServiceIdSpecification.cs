using System.Linq.Expressions;
using Core.Entities;

namespace Core.Specifications;

public class PetServiceIdSpecification : BaseSpecification<Pet>
{
    public PetServiceIdSpecification(int serivceId) : base(p => p.ServiceRecords.Any(sr => sr.Id == serivceId))
    {
        AddInclude(p => p.Owner);
        AddInclude(p => p.ServiceRecords);
        AddInclude("ServiceRecords.Service");
        AddInclude("ServiceRecords.Clinic");

    }

    public PetServiceIdSpecification()
    {
    }
}
