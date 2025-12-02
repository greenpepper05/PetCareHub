using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Services.Interface;

namespace API.Job;

public class AppointmentReminderJob(IUnitOfWork unit, IEmailService emailService)
{
    public async Task SendReminderAsync()
    {
        var now = DateTime.UtcNow;

        // Look ahead 24 hours from "now"
        var startTime = now;
        var endTime = now.AddHours(24);

        var spec = new AppointmentDateWithUserSpec(startTime, endTime);

        var appointments = await unit.Repository<Appointment>().ListAsync(spec);

        foreach (var appointment in appointments)
        {
            // Skip if no email
            if (appointment.Owner?.Email == null) continue;

            // Skip past appointments (should not send reminders)
            if (appointment.AppointmentDate <= now) continue;

            // Skip if reminder already sent in last 12 hours
            if (appointment.LastReminderSent != null &&
                appointment.LastReminderSent.Value.AddHours(12) > now)
            {
                continue;
            }

            var formattedDate = appointment.AppointmentDate.ToString("MMMM dd, yyyy (dddd)");
            var formattedTime = appointment.AppointmentDate.ToString("hh:mm tt");

            var message = $@"
                <p>Hello <strong>{appointment.Owner.FirstName} {appointment.Owner.LastName}</strong>,</p>

                <p>This is a friendly reminder about your upcoming appointment for your pet 
                <strong>{appointment.Pet?.Name}</strong>.</p>

                <table style='border-collapse: collapse; width: 100%; margin-top: 10px;'>
                    <tr>
                        <td style='padding: 8px; border: 1px solid #ddd;'>Appointment ID:</td>
                        <td style='padding: 8px; border: 1px solid #ddd;'>
                            <strong>#{appointment.Id}</strong>
                        </td>
                    </tr>
                    <tr>
                        <td style='padding: 8px; border: 1px solid #ddd;'>Pet Name:</td>
                        <td style='padding: 8px; border: 1px solid #ddd;'>
                            <strong>{appointment.Pet?.Name}</strong>
                        </td>
                    </tr>
                    <tr>
                        <td style='padding: 8px; border: 1px solid #ddd;'>Service Type:</td>
                        <td style='padding: 8px; border: 1px solid #ddd;'>
                            <strong>{appointment.Service?.Name ?? "N/A"}</strong>
                        </td>
                    </tr>
                    <tr>
                        <td style='padding: 8px; border: 1px solid #ddd;'>Clinic:</td>
                        <td style='padding: 8px; border: 1px solid #ddd;'>
                            <strong>{appointment.Clinic?.ClinicName ?? "N/A"}</strong>
                        </td>
                    </tr>
                    <tr>
                        <td style='padding: 8px; border: 1px solid #ddd;'>Appointment Date:</td>
                        <td style='padding: 8px; border: 1px solid #ddd;'>
                            <strong>{appointment.AppointmentDate:MMMM dd, yyyy (dddd)}</strong>
                        </td>
                    </tr>
                    <tr>
                        <td style='padding: 8px; border: 1px solid #ddd;'>Appointment Time:</td>
                        <td style='padding: 8px; border: 1px solid #ddd;'>
                            <strong>{appointment.AppointmentDate:hh:mm tt}</strong>
                        </td>
                    </tr>
                    <tr>
                        <td style='padding: 8px; border: 1px solid #ddd;'>Status:</td>
                        <td style='padding: 8px; border: 1px solid #ddd; color: orange;'>
                            <strong>Upcoming Appointment</strong>
                        </td>
                    </tr>
                </table>

                <p style='margin-top: 16px;'>
                    Please ensure to arrive on time. If you need to reschedule or update your appointment, 
                    feel free to contact the clinic.
                </p>

                <p>Thank you for choosing us to care for your pet!</p>

                <p>— <strong>{appointment.Clinic?.ClinicName ?? "Your Veterinary Clinic"}</strong></p>
            ";


            await emailService.SendEmailAsync(
                appointment.Owner.Email,
                "PetCareHub: Appointment Reminder",
                message
            );

            // Mark reminder time
            appointment.LastReminderSent = now;
            unit.Repository<Appointment>().Update(appointment);
        }

        await unit.Complete();

        Console.WriteLine($"[Hangfire] Appointment reminders sent at {now:yyyy-MM-dd HH:mm:ss} UTC");
    }
}
