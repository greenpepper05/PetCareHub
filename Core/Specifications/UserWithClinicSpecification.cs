using System;
using System.Linq.Expressions;
using Core.Entities;

namespace Core.Specifications;

public class UserWithClinicSpecification : BaseSpecification<AppUser>
{
    public UserWithClinicSpecification(string id) : base(a => a.Id == id)
    {
        AddInclude(c => c.Clinic!);
    }

    protected UserWithClinicSpecification()
    {
    }
}
