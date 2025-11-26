using System.Linq.Expressions;
using Core.Entities;

namespace Core.Specifications;

public class StaffByClinicSpecification : BaseSpecification<Staff>
{
    public StaffByClinicSpecification(int clinicId) : base(x => x.ClinicId == clinicId)
    {
        AddInclude(x => x.Clinic);
        AddOrderBy(x => x.LastName);
    }

    public StaffByClinicSpecification(int id, int clinicId) : base(x => x.Id == id && x.ClinicId == clinicId)
    {
        AddInclude(x => x.Clinic);
    }
}
