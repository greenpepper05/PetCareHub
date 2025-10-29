using System;
using System.Linq.Expressions;
using Core.Entities;

namespace Core.Specifications;

public class AppointmentPaginatedSpecification : BaseSpecification<Appointment>
{
    public AppointmentPaginatedSpecification(AppointmentSpecParams specParams, int clinicId) : base(a => a.ClinicId == clinicId)
    {
        AddInclude(x => x.Pet!);
        AddInclude(x => x.Service!);
        AddInclude(x => x.Clinic!);
        AddInclude(x => x.Owner!);
    }

    protected AppointmentPaginatedSpecification()
    {
    }
}
