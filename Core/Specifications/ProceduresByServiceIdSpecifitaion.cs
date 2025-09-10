using System.Linq.Expressions;
using Core.Entities;

namespace Core.Specifications;

public class ProceduresByServiceIdSpecifitaion : BaseSpecification<Procedure>
{
    public ProceduresByServiceIdSpecifitaion(int serviceId) : base(s => s.ServiceId == serviceId)
    {
        
    }

    public ProceduresByServiceIdSpecifitaion()
    {
    }
}
