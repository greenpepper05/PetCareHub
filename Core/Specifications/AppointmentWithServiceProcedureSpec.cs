using System.Linq.Expressions;
using Core.Entities;

namespace Core.Specifications;

public class AppointmentWithServiceProcedureSpec : BaseSpecification<Appointment>
{
    public AppointmentWithServiceProcedureSpec(int id) : base(a => a.Id == id)
    {
        AddInclude(a => a.Service!);
        AddInclude(a => a.Procedures!);
        AddInclude(a => a.ProcedureSteps!);
    }
}
