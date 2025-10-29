using API.DTOs;
using API.Extensions;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

// [Authorize(Roles = "Admin")]
public class AdminController(IUnitOfWork unit) : BaseApiController
{

    // GET LIST OF ALL PETS

    // [HttpGet]
    // public async Task<ActionResult> GetAllPets()
    // {
    // }

    // ------------------------------------------

    // GET PETS BY ID

    // [HttpGet("{id:int}")]
    // public async Task<ActionResult<>> GetPetById(int id)
    // {

    // }

    // [HttpGet]
    // public async Task<ActionResult<IReadOnlyList<AppointmentDto>>> GetAppointmentByClinic()
    // {
    //     var user = await userManager.GetUserByEmail(User);
    //     if (user == null) return Unauthorized();

    //     var clinicId = user.ClinicId;

    //     var spec = new AppointmentByClinicIdSpec(clinicId);
    //     var appointments = await unit.Repository<Appointment>().ListAsync(spec);

    //     var query = appointments.Select(a => new AppointmentDto
    //     {
    //         Id = a.Id,
    //         ServiceId = a.ServiceId,
    //         AppointmentDate = a.AppointmentDate,
    //         PetId = a.PetId,
    //         Notes = a.Notes,
    //         OwnerId = a.OwnerId
    //     }).ToList();

    //     return Ok(query);
    // }


    // GET ALL SERVICES

    [HttpGet("services")]
    public async Task<ActionResult<IReadOnlyList<ServiceDto>>> GetServices()
    {
        var services = await unit.Repository<Service>().ListAllAsync();
        return Ok(services);
    }

    // GET SINGLE SERVICE

    [HttpGet("{id}")]
    public async Task<ActionResult<ServiceDto>> GetServiceById(int id)
    {
        var service = await unit.Repository<Service>().GetByIdAsync(id);
        if (service == null) return NotFound();

        return Ok(new ServiceDto
        {
            Name = service.Name,
            Description = service.Description,
            Price = service.Price
        });
    }

    // CREATE SERVICE

    [HttpPost]
    public async Task<ActionResult<ServiceDto>> CreateService(CreateServiceDto dto)
    {
        var service = new Service
        {
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
        };

        unit.Repository<Service>().Add(service);

        if (await unit.Complete())
        {
            return CreatedAtAction("GetServiceById", new { id = service.Id }, new ServiceDto
            {
                Id = service.Id,
                Name = service.Name,
                Description = service.Description,
                Price = service.Price
            });
        }

        return BadRequest("Problem Creating service");

    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, ServiceDto dto)
    {
        var existing = await unit.Repository<Service>().GetByIdAsync(id);
        if (existing == null) return NotFound("No serice found");

        var service = new Service
        {
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
        };

        unit.Repository<Service>().Update(service);

        if (await unit.Complete()) return NoContent();

        return BadRequest("Problem updating service");


    }

    // DELETE SERVICE

    [HttpDelete("{id}")]
    public async Task<ActionResult> UpdateService(int id, ServiceDto dto)
    {
        var service = await unit.Repository<Service>().GetByIdAsync(id);
        if (service == null) return NotFound("Can't find this service");

        unit.Repository<Service>().Remove(service);
        return NoContent();
    }

    

}
