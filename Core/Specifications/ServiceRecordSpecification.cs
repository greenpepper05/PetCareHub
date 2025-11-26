using System.Linq.Expressions;
using Core.Entities;

namespace Core.Specifications;

public class ServiceRecordSpecification : BaseSpecification<ServiceRecord>
{
    public ServiceRecordSpecification(int id) : base(x => x.Id == id)
    {
        AddInclude(x => x.Pet!);
        AddInclude(x => x.Service!);
        AddInclude(x => x.Clinic!);
        AddInclude(x => x.Pet!.Owner);
    }
    public ServiceRecordSpecification(int petId, int appointmentId) : base(s => s.PetId == petId && s.AppointmentId == appointmentId)
    {
        AddInclude(x => x.Pet!);
        AddInclude(x => x.Service!);
        AddInclude(x => x.Clinic!);
    }

    public ServiceRecordSpecification()
    {
    }
}
