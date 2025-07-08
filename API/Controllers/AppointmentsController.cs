using API.DTOs;
using API.Extensions;
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
        var user = await userManager.GetUserByEmail(User);

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

        return Ok(new
        {
            Service = appointmentDto.ServiceName,
            Date = appointmentDto.AppointmentDate,
            Pet = appointmentDto.PetId,
            OwnerId = user.Id,
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
        var appointments = await unit.Repository<Appointment>().ListAllAsync();

        return Ok(appointments);
    }
}
