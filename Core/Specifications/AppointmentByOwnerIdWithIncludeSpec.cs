using System;
using System.Linq.Expressions;
using Core.Entities;

namespace Core.Specifications;

public class AppointmentByOwnerIdWithIncludeSpec : BaseSpecification<Appointment>
{
    public AppointmentByOwnerIdWithIncludeSpec(string ownerId) : base(x => x.OwnerId == ownerId)
    {
        AddInclude(a => a.Pet!);
        AddInclude(a => a.Service!);
    }
}
