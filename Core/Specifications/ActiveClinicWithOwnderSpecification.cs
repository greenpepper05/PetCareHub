using System.Linq.Expressions;
using Core.Entities;

namespace Core.Specifications;

public class ActiveClinicWithOwnderSpecification : BaseSpecification<Clinic>
{
    public ActiveClinicWithOwnderSpecification(Expression<Func<Clinic, bool>>? criteria) : base(criteria)
    {
    }

    public ActiveClinicWithOwnderSpecification() : base(x => x.Status == "Active")
    {
        AddInclude(x => x.Owner!);
    }
}
