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
    public async Task<ActionResult> CreateAppointment(CreateAppointmentDto appointmentDto)
    {
        var user = await userManager.GetUserByEmail(User);

        if (user == null) return NotFound("User not found");

        var appointment = new Appointment
        {
            ServiceName = appointmentDto.ServiceName,
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

        return Ok(new AppointmentDto
        {
            Id = appointment.Id,
            ServiceName = appointment.ServiceName,
            AppointmentDate = appointment.AppointmentDate,
            PetId = appointment.PetId,
            OwnerId = appointment.OwnerId,
            Notes = appointment.Notes,
            ClinicId = appointmentDto.ClinicId
        });
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Appointment>> GetAppointmentById(int id)
    {
        var appointment = await unit.Repository<Appointment>().GetByIdAsync(id);

        return Ok(appointment);
    }

    // GET ALL SERVICES

    [HttpGet("services")]
    public async Task<ActionResult<IReadOnlyList<Service>>> GetServices()
    {
        var services = await unit.Repository<Service>().ListAllAsync();
        return Ok(services);
    }

    // GET ALL APPOINTMENTS

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Appointment>>> GetAppointments()
    {
        var user = await userManager.GetUserByEmail(User);
        if (user == null) return Unauthorized();

        var spec = new AppointmentByOwnerIdSpec(user.Id);

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
