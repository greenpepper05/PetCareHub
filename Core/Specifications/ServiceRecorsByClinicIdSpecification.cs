using System;
using System.Linq.Expressions;
using Core.Entities;

namespace Core.Specifications;

public class ServiceRecorsByClinicIdSpecification : BaseSpecification<ServiceRecord>
{
    public ServiceRecorsByClinicIdSpecification(int? clinicId, DateTime date) :
        base(s => s.ClinicId == clinicId && s.DateOfService.Date == date.Date)
    {
        AddInclude(x => x.Pet!);
        AddInclude(x => x.Service!);
        AddInclude(x => x.Clinic!);
    }

    public ServiceRecorsByClinicIdSpecification()
    {
    }
}
