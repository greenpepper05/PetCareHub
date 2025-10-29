using System;
using System.Linq.Expressions;
using Core.Entities;

namespace Core.Specifications;

public class ProceduresByServiceAndIdSpecification : BaseSpecification<Procedure>
{
    public ProceduresByServiceAndIdSpecification(int serviceId, IEnumerable<int> procedureIds) :
        base(p => p.ServiceId == serviceId && procedureIds.Contains(p.Id))
    {
    }

    public ProceduresByServiceAndIdSpecification()
    {
    }
}
