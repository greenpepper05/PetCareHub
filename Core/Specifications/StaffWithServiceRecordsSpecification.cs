using System.Linq.Expressions;
using Core.Entities;

namespace Core.Specifications;

public class StaffWithServiceRecordsSpecification : BaseSpecification<ServiceRecord>
{
    public StaffWithServiceRecordsSpecification(int staffId, int clinicId) : 
        base(sr => sr.StaffId == staffId && sr.ClinicId == clinicId)
    {
        AddInclude(sr => sr.Pet!);
        AddInclude(sr => sr.Service!);
        AddInclude(sr => sr.Clinic!);
        AddInclude(sr => sr.Staff!);
        AddInclude(sr => sr.Steps);
    }

    protected StaffWithServiceRecordsSpecification()
    {
    }
}
