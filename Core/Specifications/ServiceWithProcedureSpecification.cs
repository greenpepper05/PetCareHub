using System;
using System.Linq.Expressions;
using Core.Entities;

namespace Core.Specifications;

public class ServiceWithProcedureSpecification : BaseSpecification<Service>
{
    public ServiceWithProcedureSpecification(int serviceId) : base(s => s.Id == serviceId)
    {
        AddInclude(s => s.Procedures);
    }

    public ServiceWithProcedureSpecification()
    {
    }
}
