using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Services.Interface;

namespace API.Job;

public class AppointmentReminderJob(IUnitOfWork unit, IEmailService emailService)
{
    public async Task SendReminderAsync()
    {
        var startTime = DateTime.Today;
        var endTime = startTime.AddHours(24);

        var spec = new AppointmentDateWithUserSpec(startTime, endTime);

        var appointments = await unit.Repository<Appointment>().ListAsync(spec);

        foreach (var appointment in appointments)
        {
            if (appointment.Owner?.Email == null) continue;

            var message = $"Hello <strong>{appointment.Owner.FirstName} {appointment.Owner.LastName}</strong>, \n\n" +
                          $"This is a reminder that your pet <strong>{appointment.Pet?.Name}</strong> " +
                          $"appointment is scheduled on {appointment.AppointmentDate:MMMM dd, yyyy} " +
                          $"at **{appointment.AppointmentDate:hh:mm tt}**.";

            await emailService.SendEmailAsync(appointment.Owner.Email, "PetCareHub Appointment Reminder", message);
        }

        Console.WriteLine($"[Hangfire] Found {appointments.Count} appointment(s) scheduled between {startTime:hh:mm tt} and {endTime:hh:mm tt}.");
    }
}
