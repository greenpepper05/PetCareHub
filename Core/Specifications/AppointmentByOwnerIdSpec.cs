using Core.Entities;

namespace Core.Specifications;

public class AppointmentByOwnerIdSpec : BaseSpecification<Appointment>
{
    public AppointmentByOwnerIdSpec(string onwerId) : base(x => x.OwnerId == onwerId)
    {
        AddInclude(x => x.Pet!);
        AddInclude(x => x.Service!);
        AddInclude(x => x.Clinic!);
        AddInclude(x => x.Owner!);

        AddOrderByDescending(x => x.AppointmentDate);
    }
}
