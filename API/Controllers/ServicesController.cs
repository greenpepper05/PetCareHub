using API.DTOs;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class ServicesController(IUnitOfWork unit, UserManager<AppUser> userManager) : BaseApiController
{

    // GET ALL SERVICES

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Service>>> GetServices()
    {
        var services = await unit.Repository<Service>().ListAllAsync();
        return Ok(services);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<IReadOnlyList<Service>>> GetService(int id)
    {
        var service = await unit.Repository<Service>().GetByIdAsync(id);

        return Ok(service);
    }

    // GET SERVICES BY CLINIC

    [Authorize(Roles = "Admin")]
    [HttpGet("clinic")]
    public async Task<ActionResult<IReadOnlyList<Service>>> GetServicesByClinic()
    {
        var user = await userManager.GetUserAsync(User);
        if (user == null) return NotFound();

        if (!user.ClinicId.HasValue) return NotFound();

        var spec = new ServicesByClinicIdSpecification(user.ClinicId.Value);

        var services = await unit.Repository<Service>().ListAsync(spec);

        return Ok(services);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult> CreateService([FromBody] ServiceDto dto)
    {
        var user = await userManager.GetUserAsync(User);
        if (user == null) return NotFound();

        if (!user.ClinicId.HasValue) return NotFound();

        var service = new Service
        {
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            ClinicId = user.ClinicId.Value
        };

        unit.Repository<Service>().Add(service);
        await unit.Complete();

        return Ok(new ServiceDto
        {
            Id = service.Id,
            Name = service.Name,
            Description = service.Description,
            Price = service.Price
        });
    }
}
