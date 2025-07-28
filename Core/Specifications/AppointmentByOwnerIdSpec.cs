using System.Linq.Expressions;
using Core.Entities;

namespace Core.Specifications;

public class AppointmentByOwnerIdSpec : BaseSpecification<Appointment>
{
    public AppointmentByOwnerIdSpec(string onwerId) : base(x => x.OwnerId == onwerId)
    {
    }
}
