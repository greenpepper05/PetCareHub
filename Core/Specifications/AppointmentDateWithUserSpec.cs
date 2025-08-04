using System.Linq.Expressions;
using Core.Entities;

namespace Core.Specifications;

public class AppointmentDateWithUserSpec : BaseSpecification<Appointment>
{
    public AppointmentDateWithUserSpec(DateTime fromDate, DateTime toDate) : base(a => a.AppointmentDate >= fromDate && a.AppointmentDate < toDate)
    {
        AddInclude(u => u.Owner!);
        AddInclude(p => p.Pet!);
    }
}
