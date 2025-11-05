using API.DTOs;
using API.Extensions;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;


public class ClinicController(IUnitOfWork unit,
    UserManager<AppUser> userManager, IMapper mapper) : BaseApiController
{

    // [Authorize(Roles = "SuperAdmin")]
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ClinicDto>>> GetClinic()
    {
        var spec = new ClinicWithOwnerSpecification();

        var clinic = await unit.Repository<Clinic>().ListAsync(spec);

        var data = mapper.Map<IReadOnlyList<ClinicDto>>(clinic);
        
        return Ok(data);
    }

    [Authorize(Roles = "SuperAdmin")]
    [HttpPost("register")]
    public async Task<ActionResult> RegisterClinic([FromBody] ClinicDto dto)
    {

        var clinic = new Clinic
        {
            OwnerId = dto.OwnerId,
            ClinicName = dto.ClinicName,
            Address = dto.Address,
            PhoneNumber = dto.PhoneNumber,
            Email = dto.Email,
            PictureUrl = dto.PictureUrl
        };

        unit.Repository<Clinic>().Add(clinic);

        if (!await unit.Complete())
        {
            return BadRequest("Failed to create clinic.");
        }
        
        var user = await userManager.Users.FirstOrDefaultAsync(u => u.Id == clinic.OwnerId);

        if (user == null) return NotFound("Clinic created, but the specified Owner user was not found.");

        user.ClinicId = clinic.Id;

        var userUpdateResult = await userManager.UpdateAsync(user);

        if (!userUpdateResult.Succeeded) return BadRequest("Clinic created, but failed to assign ClinicId to the Owner user.");

        return Ok(new ClinicDto
        {
            Id = clinic.Id,
            OwnerId = clinic.OwnerId,
            Address = dto.Address,
            PhoneNumber = dto.PhoneNumber,
            Email = dto.Email,
            PictureUrl = dto.PictureUrl
        });

    }

    [Authorize(Roles = "SuperAdmin")]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Clinic>> GetClinicById(int id)
    {
        var spec = new ClinicWithOwnerSpecification(id);

        var clinic = await unit.Repository<Clinic>().GetEntityWithSpec(spec);

        if (clinic == null) return NotFound();

        var clinicDto = mapper.Map<ClinicDto>(clinic);

        return Ok(clinicDto);
    }

    [HttpGet("info/{id}")]
    public async Task<ActionResult<ClinicDto>> GetClinicInfo(int id)
    {
        var spec = new ClinicWithOwnerSpecification(id);

        var clinic = await unit.Repository<Clinic>().GetEntityWithSpec(spec);

        if (clinic == null) return NotFound();

        var clinicDto = mapper.Map<ClinicDto>(clinic);

        return Ok(clinicDto);
    }
    
    [Authorize(Roles = "SuperAdmin")]
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteClinic(int id)
    {
       
        var clinic = await unit.Repository<Clinic>().GetByIdAsync(id);

        if (clinic == null) return NotFound();

        var ownerId = clinic.OwnerId;

        var user = await userManager.Users.FirstOrDefaultAsync(a => a.Id == ownerId);

        if (user == null) return NotFound();

        user.ClinicId = null;

        unit.Repository<Clinic>().Remove(clinic);

        if (await unit.Complete()) return NoContent();

        return BadRequest("Failed to delete the clinic!");
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("admin")]
    public async Task<ActionResult> GetClinicAdmin()
    {
        var user = await userManager.GetUserByEmail(User);
        if (user?.ClinicId == null) return Unauthorized("User not assigned to a clinic");

        var clinicId = user.ClinicId.Value;

        var clinic = await unit.Repository<Clinic>().GetByIdAsync(clinicId);

        return Ok(mapper.Map<ClinicDto>(clinic));
    }

    // GET ALL SERVICES BY CLINIC
    [Authorize(Roles = "Admin")]
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
    [Authorize(Roles = "Admin")]
    [HttpGet("services/{id:int}")]
    public async Task<ActionResult<ServiceDtoWithProcedure>> GetServiceWithProcedures(int id)
    {
        var user = await userManager.GetUserByEmail(User);
        if (user?.ClinicId == null) return Unauthorized("User not assigned to a clinic");

        var clinicId = user.ClinicId.Value;

        var spec = new ServiceWithProceduresSpecification(id, clinicId);
        var service = await unit.Repository<Service>().GetEntityWithSpec(spec);

        if (service == null) return NotFound();

        return Ok(new ServiceDtoWithProcedure
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

    [Authorize(Roles = "Admin")]
    [HttpPost("services/{serviceId}/procedures")]
    public async Task<ActionResult> AddProcedure(int serviceId, [FromBody] ProcedureDto dto)
    {
        var user = await userManager.GetUserByEmail(User);
        if (user?.ClinicId == null) return Unauthorized("User not assigned to a clinic");

        var spec = new ServiceWithProceduresSpecification(serviceId, user.ClinicId.Value);
        var service = await unit.Repository<Service>().GetEntityWithSpec(spec);
        if (service == null || service.ClinicId != user.ClinicId) return NotFound();

        var lastOrder = service.Procedures.Count != 0 ? service.Procedures.Max(p => p.Order) : -1;
        var nextOrder = lastOrder + 1;

        var procedure = new Procedure
        {
            ServiceId = serviceId,
            Name = dto.Name,
            Description = dto.Description,
            Order = nextOrder
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

    [Authorize(Roles = "Admin")]
    [HttpGet("services/{serviceId}/procedures")]
    public async Task<ActionResult<IReadOnlyList<ProcedureDto>>> GetProcedure(int serviceId)
    {
        var user = await userManager.GetUserByEmail(User);
        if (user?.ClinicId == null) return Unauthorized("User not assigned to a clinic");

        var service = await unit.Repository<Service>().GetByIdAsync(serviceId);
        if (service == null || service.ClinicId != user.ClinicId) return NotFound();

        var spec = new ProceduresByServiceIdSpecification(serviceId);
        var procedures = await unit.Repository<Procedure>().ListAsync(spec);

        // if (procedures == null || !procedures.Any()) return NotFound("No procedures found for this service");

        var data = mapper.Map<IReadOnlyList<ProcedureDto>>(procedures);

        return Ok(data);

    }

    [Authorize(Roles = "Admin")]
    [HttpPut("services/{serviceId}/procedures/update")]
    public async Task<ActionResult> UpdateProcedures(int serviceId, [FromBody] List<ProcedureDto> dtos)
    {
        var user = await userManager.GetUserByEmail(User);
        if (user?.ClinicId == null) return Unauthorized("User not assigned to a clinic");

        var service = await unit.Repository<Service>().GetByIdAsync(serviceId);
        if (service == null || service.ClinicId != user.ClinicId) return NotFound();

        foreach (var dto in dtos)
        {
            var proc = await unit.Repository<Procedure>().GetByIdAsync(dto.Id);
            if (proc == null || proc.ServiceId != serviceId) continue;

            proc.Name = dto.Name;
            proc.Description = dto.Description;
            proc.Order = dto.Order;
            proc.ServiceId = dto.ServiceId;

        }

        await unit.Complete();
        return Ok("Updated");
    }

    [Authorize(Roles = "Admin")]
    [HttpPatch("services/{serviceId}/procedures/reorder")]
    public async Task<ActionResult> ReOrderProcedure(int serviceId, [FromBody] List<ReOrderProcedureDto> dtos)
    {
        var user = await userManager.GetUserByEmail(User);
        if (user?.ClinicId == null) return Unauthorized("User not assigned to a clinic");

        var service = await unit.Repository<Service>().GetByIdAsync(serviceId);

        if (service == null || service.ClinicId != user.ClinicId) return NotFound();

        var procedureIds = dtos.Select(d => d.Id).ToList();
        var spec = new ProceduresByServiceAndIdSpecification(serviceId, procedureIds);
        var procedures = await unit.Repository<Procedure>().ListAsync(spec);

        if (procedures.Count != procedureIds.Count)
            return NotFound("One or more procedures not found for the given service.");

        foreach (var proc in procedures)
        {
            proc.Order = -proc.Id;
        }
        await unit.Complete();

        foreach (var dto in dtos)
        {
            var procToUpdate = procedures.FirstOrDefault(p => p.Id == dto.Id);
            if (procToUpdate != null)
            {
                procToUpdate.Order = dto.Order;
            }

        }
        await unit.Complete();
        return Ok("Reordering Complete");
    }


    [Authorize(Roles = "Admin")]
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

    [Authorize(Roles = "Admin")]
    [HttpPatch("procedures/{id:int}")]
    public async Task<ActionResult> UpdateProcedure(int id, [FromBody] UpdateProcedureDto dto)
    {
        var user = await userManager.GetUserByEmail(User);

        if (user == null) return Unauthorized();

        var procedure = await unit.Repository<Procedure>().GetByIdAsync(id);
        if (procedure == null) return NotFound();

        procedure.Name = dto.Name;
        procedure.Description = dto.Description;

        unit.Repository<Procedure>().Update(procedure);

        await unit.Complete();

        return Ok("Procedure updated successfuly!");
    }

}
