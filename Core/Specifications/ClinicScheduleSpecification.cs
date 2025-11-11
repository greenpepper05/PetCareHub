using System.Linq.Expressions;
using Core.Entities;

namespace Core.Specifications;

public class ClinicScheduleSpecification : BaseSpecification<ClinicSchedule>
{
    public ClinicScheduleSpecification(int id) : base(s => s.ClinicId == id)
    {
        AddInclude(x => x.Clinic);
    }

    protected ClinicScheduleSpecification()
    {
    }
}
