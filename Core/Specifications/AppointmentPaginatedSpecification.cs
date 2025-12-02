using System;
using System.Linq.Expressions;
using Core.Entities;
using Core.Models;

namespace Core.Specifications;

public class AppointmentPaginatedSpecification : BaseSpecification<Appointment>
{
    public AppointmentPaginatedSpecification(AppointmentSpecParams specParams, int clinicId, DateTime now) : base(a => a.ClinicId == clinicId && a.AppointmentDate >= now && a.Status == AppointmentStatus.Pending)
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
