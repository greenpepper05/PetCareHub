using System;
using System.Linq.Expressions;
using Core.Entities;

namespace Core.Specifications;

public class AppointmentWithUserSpec : BaseSpecification<Appointment>
{
    public AppointmentWithUserSpec(int id) : base(x => x.Id == id)
    {
        AddInclude(x => x.Owner!);
        AddInclude(x => x.Pet!);
        AddInclude(x => x.Service!);
        AddInclude(x => x.Clinic!);
    }
}
