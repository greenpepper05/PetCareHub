namespace Infrastructure.Services.Interface;

public interface IClinicStatusService
{
    Task<bool> IsClinicOpenAsync(int clinicId);
}
