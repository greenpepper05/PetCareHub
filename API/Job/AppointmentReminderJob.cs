using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Services;
using Microsoft.AspNetCore.Identity;

namespace API.Job;

public class AppointmentReminderJob(IUnitOfWork unit, IEmailService emailService)
{
    public async Task SendReminderAsync()
    {
        var today = DateTime.Today;
        var tomorrow = today.AddDays(1);

        var spec = new AppointmentDateWithUserSpec(today, tomorrow);

        var appointments = await unit.Repository<Appointment>().ListAsync(spec);

        foreach (var appointment in appointments)
        {
            if (appointment.Owner?.Email == null) continue;

            var message = $"Hello <strong>{appointment.Owner.FirstName} {appointment.Owner.LastName}</strong>, \n\nThis is a reminder that your pet <strong>{appointment.Pet?.Name}</strong> appointment scheduled on {appointment.AppointmentDate:MMMM dd, yyyy}.";
            await emailService.SendEmailAsync(appointment.Owner.Email, "PetCareHub Appointment Reminder", message);
        }

        Console.WriteLine($"[Hangfire] Found {appointments.Count} appointment(s) for today/tomorrow.");
    }
}
