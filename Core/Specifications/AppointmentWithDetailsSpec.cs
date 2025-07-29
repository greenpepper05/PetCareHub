using System.Linq.Expressions;
using Core.Entities;

namespace Core.Specifications;

public class AppointmentWithDetailsSpec : BaseSpecification<Appointment>
{
    public AppointmentWithDetailsSpec(int id) : base(a => a.Id == id)
    {
        AddInclude(a => a.Owner!);
        AddInclude(a => a.Pet!);
        AddInclude(a => a.Service!);
        AddInclude(a => a.Clinic!);
    }
}
