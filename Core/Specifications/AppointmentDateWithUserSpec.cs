using System.Linq.Expressions;
using Core.Entities;

namespace Core.Specifications;

public class AppointmentDateWithUserSpec : BaseSpecification<Appointment>
{
    public AppointmentDateWithUserSpec(DateTime startTime, DateTime endTime) 
        : base(a => a.AppointmentDate >= startTime && a.AppointmentDate < endTime)
    {
        AddInclude(u => u.Owner!);
        AddInclude(p => p.Pet!);
    }
}
