using System.Linq.Expressions;
using Core.Entities;

namespace Core.Specifications;

public class AppointmentSpec : BaseSpecification<Appointment>
{
    public AppointmentSpec(int clinicId) : base(x => x.ClinicId == clinicId)
    {
        AddInclude(x => x.Clinic!);
        AddInclude(x => x.Pet!);
        AddInclude(x => x.Service!);
    }
}
