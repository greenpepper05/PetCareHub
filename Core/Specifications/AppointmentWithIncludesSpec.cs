using Core.Entities;

namespace Core.Specifications;

public class AppointmentWithIncludesSpec : BaseSpecification<Appointment>
{
    public AppointmentWithIncludesSpec(int id) : base(a => a.Id == id)
    {
        AddInclude(a => a.Pet!);
        AddInclude(a => a.Service!);
        AddInclude(a => a.Owner!);
    }
}
