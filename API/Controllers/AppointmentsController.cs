using API.DTOs;
using Core.Entities;
using Core.Interfaces;
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
        var user = await userManager.FindByIdAsync(appointmentDto.OwnerId);

        if (user == null) return NotFound("User not found");

        var appointment = new Appointment
        {
            ServiceName = appointmentDto.ServiceName,
            AppointmentDate = appointmentDto.AppointmentDate,
            PetId = appointmentDto.PetId,
            OwnerId = user.Id,
        };

        unit.Repository<Appointment>().Add(appointment);

        if (await unit.Complete())
        {
            return CreatedAtAction("GetAppointmentById", new { id = appointment.Id }, appointment);
        }

        return Ok();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<IReadOnlyList<Appointment>>> GetAppointmentById(int id)
    {
        var appointment = await unit.Repository<Appointment>().GetByIdAsync(id);

        return Ok(appointment);
    }

    // GET ALL SERVICES

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<AppointmentService>>> GetServices()
    {
        var services = await unit.Repository<AppointmentService>().ListAllAsync();
        return Ok(services);
    }
}
