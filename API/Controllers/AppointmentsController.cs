using API.DTOs;
using API.Extensions;
using API.RequestHelpters;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class AppointmentsController(IUnitOfWork unit,
    UserManager<AppUser> userManager, IEmailService emailService, IMapper mapper) : BaseApiController
{

    // CREATE APPOINTMENTS

    [HttpPost]
    public async Task<ActionResult> CreateAppointment([FromBody] CreateAppointmentDto appointmentDto)
    {
        var user = await userManager.GetUserByEmail(User);

        if (user == null) return NotFound("User not found");

        var appointment = new Appointment
        {
            ServiceId = appointmentDto.ServiceId,
            AppointmentDate = appointmentDto.AppointmentDate,
            PetId = appointmentDto.PetId,
            OwnerId = user.Id,
            Notes = appointmentDto.Notes,
            ClinicId = appointmentDto.ClinicId,

        };

        unit.Repository<Appointment>().Add(appointment);

        if (await unit.Complete())
        {
            var apptDto = new AppointmentDto
            {
                Id = appointment.Id,
                AppointmentDate = appointment.AppointmentDate,
                Status = appointment.Status,
                PetId = appointment.PetId,
                OwnerId = appointment.OwnerId
            };

            return CreatedAtAction("GetAppointmentById", new { id = appointment.Id }, apptDto);
        }

        return BadRequest("Failed to create appointment");
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Appointment>> GetAppointmentById(int id)
    {
        var spec = new AppointmentWithIncludesSpec(id);

        var appointment = await unit.Repository<Appointment>().GetEntityWithSpec(spec);

        if (appointment == null) return NotFound();

        var appointmentDto = mapper.Map<AppointmentDto>(appointment);

        return Ok(appointmentDto);
    }

    // GET ALL APPOINTMENTS
    [Authorize(Roles = "Customer, Admin")]
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Appointment>>> GetAppointments()
    {
        var user = await userManager.GetUserByEmail(User);
        if (user == null) return Unauthorized();

        var spec = new AppointmentByOwnerIdSpec(user.Id);
        var appointments = await unit.Repository<Appointment>().ListAsync(spec);

        var data = mapper.Map<IReadOnlyList<AppointmentDto>>(appointments);

        return Ok(data);
    }

    // GET APPOINTMENT BY CLINIC ID

    [Authorize(Roles = "Admin")]
    [HttpGet("clinic/by-date/")]
    public async Task<ActionResult<IReadOnlyList<Appointment>>> GetAppointmentByClinic(
        [FromQuery] AppointmentSpecParams specParams, [FromQuery] DateTime date
    )
    {
        var user = await userManager.GetUserByEmail(User);
        if (user == null) return Unauthorized();

        if (user.ClinicId == null)
        {
            return BadRequest("This admin is not assigned to a clinic.");
        }

        var clinicId = user.ClinicId.Value;

        var spec = new AppointmentSpecification(specParams, clinicId, date);
        var countSpec = new AppointmentSpecification(specParams, clinicId, date);

        var totalItems = await unit.Repository<Appointment>().CountAsync(countSpec);
        var appointments = await unit.Repository<Appointment>().ListAsync(spec);

        var data = mapper.Map<IReadOnlyList<AppointmentDto>>(appointments);

        return Ok(new Pagination<AppointmentDto>(
            specParams.PageIndex,
            specParams.PageSize,
            totalItems,
            data
        ));
    }
    
    [Authorize(Roles = "Admin")]
    [HttpGet("{id:int}/all")]
    public async Task<ActionResult<IReadOnlyList<AppointmentDto>>> GetAllAppointmentsByClinicId(int id)
    {
        var user = await userManager.GetUserByEmail(User);
        if (user == null) return Unauthorized();

        var spec = new AppointmentByClinicIdSpec(id);

        var appointments = await unit.Repository<Appointment>().ListAsync(spec);

        var appDto = mapper.Map<IReadOnlyList<AppointmentDto>>(appointments);

        return Ok(appDto);
    }

    [HttpPatch("{id}/status")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateAppointmentStatusDto dto)
    {
        var spec = new AppointmentWithUserSpec(id);

        var appointment = await unit.Repository<Appointment>().GetEntityWithSpec(spec);

        if (appointment == null) return NotFound();

        appointment.Status = dto.Status;
        await unit.Complete();

        var subject = "";
        var message = "";

        switch (dto.Status.ToLower())
        {
            case "confirmed":
                    subject = "✅ Your Appointment Has Been Confirmed";

                    message = $@"
                        <p>Hello <strong>{appointment?.Owner?.FirstName} {appointment?.Owner?.LastName}</strong>,</p>
                        <p>We’re pleased to inform you that your appointment has been <strong>confirmed</strong>.</p>

                        <table style='border-collapse: collapse; width: 100%; margin-top: 10px;'>
                            <tr>
                                <td style='padding: 8px; border: 1px solid #ddd;'>Appointment ID:</td>
                                <td style='padding: 8px; border: 1px solid #ddd;'><strong>#{appointment?.Id}</strong></td>
                            </tr>
                            <tr>
                                <td style='padding: 8px; border: 1px solid #ddd;'>Date:</td>
                                <td style='padding: 8px; border: 1px solid #ddd;'>
                                    <strong>{appointment?.AppointmentDate:MMMM dd, yyyy (dddd)}</strong>
                                </td>
                            </tr>
                            <tr>
                                <td style='padding: 8px; border: 1px solid #ddd;'>Time:</td>
                                <td style='padding: 8px; border: 1px solid #ddd;'>
                                    <strong>{appointment?.AppointmentDate:hh:mm tt}</strong>
                                </td>
                            </tr>
                            <tr>
                                <td style='padding: 8px; border: 1px solid #ddd;'>Pet:</td>
                                <td style='padding: 8px; border: 1px solid #ddd;'>
                                    <strong>{appointment?.Pet?.Name ?? "N/A"}</strong>
                                </td>
                            </tr>
                            <tr>
                                <td style='padding: 8px; border: 1px solid #ddd;'>Service:</td>
                                <td style='padding: 8px; border: 1px solid #ddd;'>
                                    <strong>{appointment?.Service?.Name ?? "N/A"}</strong>
                                </td>
                            </tr>
                            <tr>
                                <td style='padding: 8px; border: 1px solid #ddd;'>Clinic:</td>
                                <td style='padding: 8px; border: 1px solid #ddd;'>
                                    <strong>{appointment?.Clinic?.ClinicName ?? "N/A"}</strong>
                                </td>
                            </tr>
                            <tr>
                                <td style='padding: 8px; border: 1px solid #ddd;'>Status:</td>
                                <td style='padding: 8px; border: 1px solid #ddd; color: green;'>
                                    <strong>Confirmed</strong>
                                </td>
                            </tr>
                        </table>

                        <p style='margin-top: 16px;'>Please arrive 10–15 minutes before your scheduled time.</p>
                        <p>We look forward to seeing you and your pet!</p>
                        <p>— <strong>{appointment?.Clinic?.ClinicName ?? "Your Veterinary Clinic"}</strong></p>
                    ";
                    break;
            case "cancelled":
                subject = "❌ Appointment Cancelled";

                message = $@"
                    <p>Hello <strong>{appointment?.Owner?.FirstName} {appointment?.Owner?.LastName}</strong>,</p>

                    <p>We’re sorry to inform you that your appointment has been <strong>cancelled</strong>.</p>

                    <table style='border-collapse: collapse; width: 100%; margin-top: 10px;'>
                        <tr>
                            <td style='padding: 8px; border: 1px solid #ddd;'>Appointment ID:</td>
                            <td style='padding: 8px; border: 1px solid #ddd;'><strong>#{appointment?.Id}</strong></td>
                        </tr>
                        <tr>
                            <td style='padding: 8px; border: 1px solid #ddd;'>Date:</td>
                            <td style='padding: 8px; border: 1px solid #ddd;'>
                                <strong>{appointment?.AppointmentDate:MMMM dd, yyyy (dddd)}</strong>
                            </td>
                        </tr>
                        <tr>
                            <td style='padding: 8px; border: 1px solid #ddd;'>Service:</td>
                            <td style='padding: 8px; border: 1px solid #ddd;'>
                                <strong>{appointment?.Service?.Name ?? "N/A"}</strong>
                            </td>
                        </tr>
                        <tr>
                            <td style='padding: 8px; border: 1px solid #ddd;'>Clinic:</td>
                            <td style='padding: 8px; border: 1px solid #ddd;'>
                                <strong>{appointment?.Clinic?.ClinicName ?? "N/A"}</strong>
                            </td>
                        </tr>
                        <tr>
                            <td style='padding: 8px; border: 1px solid #ddd;'>Status:</td>
                            <td style='padding: 8px; border: 1px solid #ddd; color: red;'>
                                <strong>Cancelled</strong>
                            </td>
                        </tr>
                    </table>

                    <p style='margin-top: 16px;'>If this cancellation was a mistake or you’d like to reschedule, please contact our clinic.</p>
                    <p>Thank you for your understanding.</p>
                    <p>— <strong>{appointment?.Clinic?.ClinicName ?? "Your Veterinary Clinic"}</strong></p>
                ";
            break;
        }

        var email = appointment?.Owner?.Email;

        if (!string.IsNullOrEmpty(subject) && !string.IsNullOrEmpty(message) && !string.IsNullOrWhiteSpace(email))
        {
            await emailService.SendEmailAsync(email, subject, message);

        }

        return NoContent();
    }

    // UPDATE APPOINTMENT

    [HttpPut("{id:int}")]

    public async Task<ActionResult> UpdateAppointment(int id, UpdateAppointmentDto dto)
    {
        var user = await userManager.GetUserByEmail(User);
        if (user == null) return Unauthorized();

        var appointment = await unit.Repository<Appointment>().GetByIdAsync(id);
        if (appointment == null) return NotFound();

        if (appointment.OwnerId != user.Id) return Forbid();

        appointment.AppointmentDate = dto.AppointmentDate;
        appointment.Notes = dto.Notes;

        unit.Repository<Appointment>().Update(appointment);

        if (await unit.Complete()) return NoContent();

        return BadRequest("Failed to update appointment");
    }

    // DELETE APPOINTMENT

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> CancelAppointment(int id)
    {
        var user = await userManager.GetUserByEmail(User);
        if (user == null) return Unauthorized();

        var appointment = await unit.Repository<Appointment>().GetByIdAsync(id);
        if (appointment == null) return NotFound();

        if (appointment.OwnerId != user.Id) return Forbid();

        unit.Repository<Appointment>().Remove(appointment);

        if (await unit.Complete()) return NoContent();

        return BadRequest("Failed to cancel appointment");
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("confirm/{id}")]
    public async Task<IActionResult> ConfirmAppointment(int id)
    {
        var appointment = await unit.Repository<Appointment>().GetByIdAsync(id);

        if (appointment == null) return NotFound();

        appointment.Status = "Confirmed";

        await unit.Complete();

        var user = await userManager.FindByIdAsync(appointment.OwnerId);
        await emailService.SendEmailAsync(
            user?.Email!,
            "Your Appointment is Confirmed",
            $"<p> Hi {user?.FirstName} {user?.LastName}, </p><p>Your appointment on <strong>{appointment.AppointmentDate:MMMM dd, yyyy}</strong> has been confirmed.</p>"
        );

        return Ok();
    }

    [Authorize(Roles = "SuperAdmin")]
    [HttpGet("all-confirmed")]
    public async Task<ActionResult<IReadOnlyList<AppointmentDto>>> GetAllConfirmedAppointments()
    {
        var user = await userManager.GetUserByEmail(User);

        if (user == null) return Unauthorized();

        var appointmentRemark = "Confirmed";

        var spec = new CompletedAppointmentSpecification(appointmentRemark);

        var appointmentDtos = new List<AppointmentDto>();

        var appoiuntments = await unit.Repository<Appointment>().ListAsync(spec);

        foreach (var appointment in appoiuntments)
        {
            var appointmentDto = new AppointmentDto
            {
                Status = appointment.Status
            };

            if (appointmentDto.Status == "Confirmed")
            {
                appointmentDtos.Add(appointmentDto);
            }
        }

        return Ok(appointmentDtos);
    }

    [Authorize(Roles = "SuperAdmin")]
    [HttpGet("all")]
    public async Task<ActionResult<IReadOnlyList<AppointmentDto>>> GetAllAppointments()
    {
        var user = await userManager.GetUserByEmail(User);

        if (user == null) return Unauthorized();

        var appoiuntments = await unit.Repository<Appointment>().ListAllAsync();

        var app = mapper.Map<IReadOnlyList<AppointmentDto>>(appoiuntments);

        return Ok(app);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("upcoming")]
    public async Task<ActionResult<IReadOnlyList<AppointmentDto>>> GetUpcomingAppointments([FromQuery] AppointmentSpecParams specParams, int clinicId)
    {
        var user = await userManager.GetUserByEmail(User);
        if (user == null) return Unauthorized();

        if (user.ClinicId == null)
        {
            return BadRequest("This admin is not assigned to a clinic.");
        }

        var spec = new AppointmentPaginatedSpecification(specParams, clinicId);

        var totalItems = await unit.Repository<Appointment>().CountAsync(spec);

        var appointments = await unit.Repository<Appointment>().ListAsync(spec);

        var data = mapper.Map<IReadOnlyList<AppointmentDto>>(appointments);

        return Ok(new Pagination<AppointmentDto>(
            specParams.PageIndex,
            specParams.PageSize,
            totalItems,
            data
        ));
    }

}
