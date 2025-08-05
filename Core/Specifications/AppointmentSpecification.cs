using System.Linq.Expressions;
using Core.Entities;

namespace Core.Specifications;

public class AppointmentSpecification : BaseSpecification<Appointment>
{
    public AppointmentSpecification(AppointmentSpecParams specParams, int clinicId) : base(x => x.ClinicId == clinicId)
    {
        ApplyPaging(specParams.PageSize * (specParams.PageIndex - 1), specParams.PageSize);

        switch (specParams.Sort)
        {
            case "latestToOldest":
                AddOrderByDescending(x => x.AppointmentDate);
                break;
            case "oldestToLates":
                AddOrderBy(x => x.AppointmentDate);
                break;
            case "name":
                AddOrderByDescending(x => x.Pet!.Name);
                break;
            default:
                AddOrderByDescending(x => x.AppointmentDate);
                break;
        }

        AddInclude(x => x.Pet!);
        AddInclude(x => x.Service!);
        AddInclude(x => x.Clinic!);
        AddInclude(x => x.Owner!);
    }

    protected AppointmentSpecification()
    {
    }
}
