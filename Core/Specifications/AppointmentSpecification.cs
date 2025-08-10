using System.Linq.Expressions;
using Core.Entities;

namespace Core.Specifications;

public class AppointmentSpecification : BaseSpecification<Appointment>
{
    public AppointmentSpecification(AppointmentSpecParams specParams, int clinicId, DateTime date) : base(x => x.ClinicId == clinicId && x.AppointmentDate.Date == date.Date)
    {
        ApplyPaging(specParams.PageSize * (specParams.PageIndex - 1), specParams.PageSize);

        AddInclude(x => x.Pet!);
        AddInclude(x => x.Service!);
        AddInclude(x => x.Clinic!);
        AddInclude(x => x.Owner!);
    }

    protected AppointmentSpecification()
    {
    }
}
