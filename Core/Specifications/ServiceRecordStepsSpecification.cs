using Core.Entities;

namespace Core.Specifications;

public class ServiceRecordStepsSpecification : BaseSpecification<ServiceRecordStep>
{
    public ServiceRecordStepsSpecification(int recordId) : base(s => s.ServiceRecordId == recordId)
    {
        AddInclude(s => s.Procedure!);
    }

    public ServiceRecordStepsSpecification()
    {
    }
}
