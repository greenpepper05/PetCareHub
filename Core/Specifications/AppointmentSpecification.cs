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

    public AppointmentSpecification(AppointmentSpecParams specParams, int clinicId) 
        : base(BuildCriteria(specParams, clinicId))
    {
        ApplyPaging(specParams.PageSize * (specParams.PageIndex - 1), specParams.PageSize);

        AddInclude(x => x.Pet!);
        AddInclude(x => x.Service!);
        AddInclude(x => x.Clinic!);
        AddInclude(x => x.Owner!);
    }

    private static Expression<Func<Appointment, bool>> BuildCriteria        (AppointmentSpecParams specParams, int clinicId)
    {
        int? searchId = null;

        if (!string.IsNullOrEmpty(specParams.Search) && int.TryParse(specParams.Search, out int id)) searchId = id;

        return x => 
            x.ClinicId == clinicId && 
            (
                string.IsNullOrEmpty(specParams.Search) ||
                x.Pet!.Name.Contains(specParams.Search) ||
                x.Owner!.LastName!.Contains(specParams.Search) ||
                x.Service!.Name.Contains(specParams.Search) ||
                (searchId.HasValue && x.Id == searchId.Value)
            );
    }
}
