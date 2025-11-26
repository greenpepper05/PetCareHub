using System;
using System.Linq.Expressions;
using Core.Entities;

namespace Core.Specifications;

public class BookedSlotSpecification : BaseSpecification<Appointment>
{
    public BookedSlotSpecification(int clinicId, int year, int month, int day) 
        : base(x => x.ClinicId == clinicId 
            && x.AppointmentDate.Year == year 
            && x.AppointmentDate.Month == month 
            && x.AppointmentDate.Day == day)
    {
        
    }

    public BookedSlotSpecification()
    {
    }
}
