
using System.Linq;
using Core.Entities;

namespace Core.Specifications;

public class ServiceRecordWithStepsSpecification : BaseSpecification<ServiceRecord>
{
    public ServiceRecordWithStepsSpecification(int recordId) : base(r => r.Id == recordId)
    {
        AddInclude(r => r.Steps.OrderBy(s => s.Order));
    }

    public ServiceRecordWithStepsSpecification()
    {
    }
}
