using System;
using System.Linq.Expressions;
using Core.Entities;

namespace Core.Specifications;

public class ClinicWithOwnerSpecification : BaseSpecification<Clinic>
{
    public ClinicWithOwnerSpecification(int id) : base(c => c.Id == id)
    {
        AddInclude(c => c.Owner!);
    }

    public ClinicWithOwnerSpecification() : base()
    {
        AddInclude(c => c.Owner!);
        AddInclude(c => c.Schedules);
    }
}
