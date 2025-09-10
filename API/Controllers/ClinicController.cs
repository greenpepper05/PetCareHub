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

[Authorize(Roles = "Admin")]
public class ClinicController(IUnitOfWork unit,
    UserManager<AppUser> userManager, IMapper mapper) : BaseApiController
{

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Clinic>>> GetClinic()
    {
        var clinic = await unit.Repository<Clinic>().ListAllAsync();

        return Ok(clinic);
    }

    // GET ALL SERVICES BY CLINIC

    [HttpGet("services")]
    public async Task<ActionResult<IReadOnlyList<ServiceDto>>> GetServicesForClinic()
    {
        var user = await userManager.GetUserByEmail(User);
        if (user?.ClinicId == null) return Unauthorized("User not assigned to a clinic");

        var clinicId = user.ClinicId.Value;

        var spec = new ServiceWithProceduresSpecification(clinicId);

        var services = await unit.Repository<Service>().ListAsync(spec);

        return Ok(mapper.Map<IReadOnlyList<ServiceDto>>(services));
    }

    // GET SERVICE BY ID

    [HttpGet("services/{id:int}")]
    public async Task<ActionResult<ServiceDto>> GetServiceWithProcedures(int id)
    {
        var user = await userManager.GetUserByEmail(User);
        if (user?.ClinicId == null) return Unauthorized("User not assigned to a clinic");

        var clinicId = user.ClinicId.Value;

        var spec = new ServiceWithProceduresSpecification(id, clinicId);
        var service = await unit.Repository<Service>().GetEntityWithSpec(spec);

        if (service == null) return NotFound();

        return Ok(new ServiceDto
        {
            Id = service.Id,
            Name = service.Name,
            Description = service.Description,
            Price = service.Price,
            Procedures = service.Procedures
                .OrderBy(p => p.Order)
                .Select(p => new ProcedureDto
                {
                    Id = p.Id,
                    ServiceId = p.ServiceId,
                    Name = p.Name,
                    Description = p.Description,
                    Order = p.Order
                }).ToList()
        });
    }

    [HttpPost("services/{serviceId}/procedures")]
    public async Task<ActionResult> AddProcedure(int serviceId, [FromBody] ProcedureDto dto)
    {
        var user = await userManager.GetUserByEmail(User);
        if (user?.ClinicId == null) return Unauthorized("User not assigned to a clinic");

        var service = await unit.Repository<Service>().GetByIdAsync(serviceId);
        if (service == null || service.ClinicId != user.ClinicId) return NotFound();

        var procedure = new Procedure
        {
            ServiceId = serviceId,
            Name = dto.Name,
            Description = dto.Description,
            Order = dto.Order
        };

        unit.Repository<Procedure>().Add(procedure);

        await unit.Complete();

        return Ok(new ServiceDto
        {
            Id = service.Id,
            Name = service.Name,
            Description = service.Description,
            Price = service.Price,
        });
    }


    [HttpGet("services/{serviceId}/procedures")]
    public async Task<ActionResult<IReadOnlyList<ProcedureDto>>> GetProcedure(int serviceId)
    {
        var user = await userManager.GetUserByEmail(User);
        if (user?.ClinicId == null) return Unauthorized("User not assigned to a clinic");

        var service = await unit.Repository<Service>().GetByIdAsync(serviceId);
        if (service == null || service.ClinicId != user.ClinicId) return NotFound();

        var spec = new ProceduresByServiceIdSpecification(serviceId);
        var procedures = await unit.Repository<Procedure>().ListAsync(spec);

        if (procedures == null || !procedures.Any()) return NotFound("No procedures found for this service");

        var data = mapper.Map<IReadOnlyList<ProcedureDto>>(procedures);

        return Ok(data);

    }

    [HttpPut("services/{serviceId}/procedures/reorder")]
    public async Task<ActionResult> ReorderProcedures(int serviceId, [FromBody] List<ProcedureDto> dtos)
    {
        var user = await userManager.GetUserByEmail(User);
        if (user?.ClinicId == null) return Unauthorized("User not assigned to a clinic");

        var service = await unit.Repository<Service>().GetByIdAsync(serviceId);
        if (service == null || service.ClinicId != user.ClinicId) return NotFound();

        foreach (var dto in dtos)
        {
            var proc = await unit.Repository<Procedure>().GetByIdAsync(dto.Id);
            if (proc == null || proc.ServiceId != serviceId) continue;

            proc.Order = dto.Order;

        }

        await unit.Complete();
        return Ok("Updated");
    }

    [HttpDelete("procedures/{id:int}")]
    public async Task<ActionResult> DeleteProcedure(int id)
    {
        var user = await userManager.GetUserByEmail(User);

        if (user == null) return Unauthorized();

        var procedure = await unit.Repository<Procedure>().GetByIdAsync(id);
        if (procedure == null) return NotFound();

        unit.Repository<Procedure>().Remove(procedure);

        if (await unit.Complete()) return NoContent();

        return BadRequest("Failed to remove procedure");
    }

}
