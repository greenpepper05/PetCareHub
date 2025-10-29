using System.Linq.Expressions;
using Core.Entities;

namespace Core.Specifications;

public class AdminWithClinicSpecification : BaseSpecification<AppUser>
{
    public AdminWithClinicSpecification(string id) : base(a => a.Id == id)
    {
        AddInclude(a => a.Clinic!);
    }

    public AdminWithClinicSpecification()
    {
    }
}
