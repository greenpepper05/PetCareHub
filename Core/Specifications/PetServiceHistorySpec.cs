using System.Linq.Expressions;
using Core.Entities;

namespace Core.Specifications;

public class PetServiceHistorySpec : BaseSpecification<PetServiceHistory>
{
    public PetServiceHistorySpec(int petId) : base(x => x.PetId == petId)
    {
        AddInclude(x => x.Pet);
        AddInclude(x => x.Service);
        AddInclude(x => x.Clinic);
        AddOrderByDescending(h => h.DataOfService);
    }

    public PetServiceHistorySpec(int petId, int appointmentId) :
        base(h => h.PetId == petId && h.AppointmentId == appointmentId)
    {
        AddInclude(h => h.Appointment);
        AddInclude(h => h.Service);
        AddInclude(x => x.Clinic);
    }

    public PetServiceHistorySpec() : base()
    {
        AddInclude(x => x.Pet);
        AddInclude(x => x.Service);
        AddInclude(x => x.Clinic);
    }
}
