using System;
using System.Linq.Expressions;
using Core.Entities;

namespace Core.Specifications;

public class CompletedAppointmentSpecification : BaseSpecification<Appointment>
{
    public CompletedAppointmentSpecification(string remark) : base(a => a.Status == remark)
    {
    }

    protected CompletedAppointmentSpecification()
    {
    }
}
