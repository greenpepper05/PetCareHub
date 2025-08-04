using System.Linq.Expressions;
using Core.Entities;

namespace Core.Specifications;

public class AppointmentByClinicIdSpec : BaseSpecification<Appointment>
{
    public AppointmentByClinicIdSpec(int clinicId) : base(x => x.ClinicId == clinicId)
    {
        AddInclude(x => x.Clinic!);
        AddInclude(x => x.Pet!);
    }
}
