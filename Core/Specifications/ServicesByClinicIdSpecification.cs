using System.Linq.Expressions;
using Core.Entities;

namespace Core.Specifications;

public class ServicesByClinicIdSpecification : BaseSpecification<Service>
{
    public ServicesByClinicIdSpecification(int clinicId) : base(s => s.ClinicId == clinicId)
    {
    }

    public ServicesByClinicIdSpecification()
    {
    }
}
