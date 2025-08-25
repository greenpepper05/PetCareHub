using System.Linq.Expressions;
using Core.Entities;

namespace Core.Specifications;

public class ServiceWithProceduresSpecification : BaseSpecification<Service>
{
    public ServiceWithProceduresSpecification(int serviceId, int clinicId) :
        base(s => s.Id == serviceId && s.ClinicId == clinicId)
    {
        AddInclude(s => s.Procedures.OrderBy(p => p.Order));
        AddInclude(x => x.Procedures);
    }

    public ServiceWithProceduresSpecification(int clinicId) : base(s => s.ClinicId == clinicId)
    {
        AddInclude(x => x.Procedures);
    }
}
