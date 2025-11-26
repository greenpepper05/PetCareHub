using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Services.Interface;

namespace Infrastructure.Services;

public class ClinicStatusService(IUnitOfWork unit) : IClinicStatusService
{
    public async Task<bool> IsClinicOpenAsync(int clinicId)
    {
        var phZone = TimeZoneInfo.FindSystemTimeZoneById("Asia/Manila");
        var nowPH = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, phZone);

        var today = nowPH.DayOfWeek;
        var now = TimeOnly.FromDateTime(nowPH);

        var spec = new ClinicScheduleSpecification(clinicId, today);
        var schedule = await unit.Repository<ClinicSchedule>().GetEntityWithSpec(spec);

        var clinic = await unit.Repository<Clinic>().GetByIdAsync(clinicId);
        if (clinic == null) return false;

        bool isOpen = false;

        if (schedule != null && schedule.IsOpen)
        {
            isOpen = now >= schedule.OpeningTime && now <= schedule.ClosingTime;
        }

        clinic.IsOpen = isOpen;

        unit.Repository<Clinic>().Update(clinic);

        await unit.Complete();

        return isOpen;
    }
}
