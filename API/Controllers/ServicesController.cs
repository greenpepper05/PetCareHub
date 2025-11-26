using API.DTOs;
using API.Extensions;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class ServicesController(IUnitOfWork unit, UserManager<AppUser> userManager, IMapper mapper) : BaseApiController
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

    [HttpGet("clinic/{id:int}")]
    public async Task<ActionResult<IReadOnlyList<ServiceDto>>> GetServicesByClinicId(int id)
    {
        var spec = new ServicesByClinicIdSpecification(id);

        var services = await unit.Repository<Service>().ListAsync(spec);

        var serviceDto = mapper.Map<IReadOnlyList<ServiceDto>>(services);

        return Ok(serviceDto);
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

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteService(int id)
    {
        var user = await userManager.GetUserByEmail(User);

        if (user == null) return Unauthorized();

        var service = await unit.Repository<Service>().GetByIdAsync(id);
        if (service == null) return NotFound();

        unit.Repository<Service>().Remove(service);

        await unit.Complete();

        return Ok(new { message = "Service succefully deleted" });
    }

    

}
