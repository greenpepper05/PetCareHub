using System.Formats.Asn1;
using API.DTOs;
using API.Extensions;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class AppointmentsController(IUnitOfWork unit,
    UserManager<AppUser> userManager) : BaseApiController
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
            ClinicId = appointmentDto.ClinicId
        };

        unit.Repository<Appointment>().Add(appointment);

        if (await unit.Complete())
        {
            return CreatedAtAction("GetAppointmentById", new { id = appointment.Id }, appointment);
        }

        return BadRequest("Failed to create appointment");
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Appointment>> GetAppointmentById(int id)
    {
        var spec = new AppointmentWithIncludesSpec(id);

        var appointment = await unit.Repository<Appointment>().GetEntityWithSpec(spec);

        if (appointment == null) return NotFound();

        return Ok(appointment);
    }

    // GET ALL APPOINTMENTS

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Appointment>>> GetAppointments()
    {
        var user = await userManager.GetUserByEmail(User);
        if (user == null) return Unauthorized();

        var spec = new AppointmentByOwnerIdWithIncludeSpec(user.Id);

        var appointments = await unit.Repository<Appointment>().ListAsync(spec);

        return Ok(appointments);
    }

    // GET APPOINTMENT BY CLINIC ID

    [Authorize(Roles = "Admin")]
    [HttpGet("clinic")]
    public async Task<ActionResult<IReadOnlyList<Appointment>>> GetAppointmentByClinic()
    {
        var user = await userManager.GetUserByEmail(User);
        if (user == null) return Unauthorized();

        var spec = new AppointmentSpec(user.ClinicId);

        var appointments = await unit.Repository<Appointment>().ListAsync(spec);

        return Ok(appointments);
    }

    // UPDATE APPOINTMENT

    [HttpPut("{id}")]
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

    [HttpDelete("{id}")]
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
}
