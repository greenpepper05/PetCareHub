using API.DTOs;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class ServiceRecordController(IUnitOfWork unit,
    IMapper mapper, UserManager<AppUser> userManager) : BaseApiController
{
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ServiceRecord>> GetServiceRecordById(int id)
    {
        var spec = new ServiceRecordSpecification(id);

        var record = await unit.Repository<ServiceRecord>().GetEntityWithSpec(spec);

        return Ok(mapper.Map<ServiceRecordDto>(record));
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("by-date")]
    public async Task<ActionResult<IReadOnlyList<CreateServiceRecordDto>>> GetAll([FromQuery] DateTime date)
    {
        var user = await userManager.GetUserAsync(User);
        var clinicId = user!.ClinicId;
        var spec = new ServiceRecorsByClinicIdSpecification(clinicId, date);
        var record = await unit.Repository<ServiceRecord>().ListAsync(spec);
        return Ok(mapper.Map<IReadOnlyList<ServiceRecordDto>>(record));
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult> CreateServiceRecord([FromBody] CreateServiceRecordDto dto)
    {
        try
        {
            var record = mapper.Map<ServiceRecord>(dto);
            unit.Repository<ServiceRecord>().Add(record);
            await unit.Complete();

            var procedures = await unit.Repository<Procedure>()
                .ListAsync(new ProceduresByServiceIdSpecifitaion(dto.ServiceId));

            foreach (var proc in procedures.OrderBy(p => p.Order))
            {
                var step = new ServiceRecordStep
                {
                    ServiceRecordId = record.Id,
                    ProcedureId = proc.Id,
                    Name = proc.Name,
                    Description = proc.Description,
                    Order = proc.Order,
                    IsCompleted = false,
                    IsSkipped = false
                };

                unit.Repository<ServiceRecordStep>().Add(step);
            }

            await unit.Complete();

            return Ok(new { record.Id });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                message = "Error while saving to database",
                error = ex.InnerException?.Message ?? ex.Message
            });
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("records/{recordId}/steps")]
    public async Task<ActionResult<IReadOnlyList<ServiceRecordStepDto>>> GetSteps(int recordId)
    {
        var spec = new ServiceRecordWithStepsSpecification(recordId);
        var record = await unit.Repository<ServiceRecord>().GetEntityWithSpec(spec);

        if (record == null) return NotFound();

        var data = mapper.Map<IReadOnlyList<ServiceRecordStepDto>>(record.Steps);

        return Ok(data);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("records/{recordId}/steps/{stepId}/complete")]
    public async Task<ActionResult> CompleteStep(int recordId, int stepId)
    {
        var step = await unit.Repository<ServiceRecordStep>().GetByIdAsync(stepId);
        if (step == null || step.ServiceRecordId != recordId) return NotFound();

        step.IsCompleted = true;
        step.CompletedAt = DateTime.UtcNow;

        unit.Repository<ServiceRecordStep>().Update(step);
        await unit.Complete();

        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("records/{recordId}/steps/{stepId}/skip")]
    public async Task<ActionResult> SkipStep(int recordId, int stepId)
    {
        var step = await unit.Repository<ServiceRecordStep>().GetByIdAsync(stepId);
        if (step == null || step.ServiceRecordId != recordId) return NotFound();

        step.IsSkipped = true;

        unit.Repository<ServiceRecordStep>().Update(step);
        await unit.Complete();

        return NoContent();
    }

}
