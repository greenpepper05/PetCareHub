using API.DTOs;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;


namespace API.Controllers;

public class ServiceProcedureController(IUnitOfWork unit) : BaseApiController
{

    // [HttpGet]
    // public async Task<ActionResult<IReadOnlyList<ServiceProcedureDto>>> GetProcedures(int appointmentServiceId)
    // {
    //     var procedures = await unit.Repository<ServiceProcedure>()
    //         .ListAsync(new SpecificationByForeignKey<ServiceProcedure>(appointmentServiceId, "AppointmentServiceId"));
    //     return Ok(procedures.Select(p => new ServiceProcedureDto
    //     {
    //         Id = p.Id,
    //         StepName = p.StepName,
    //         IsCompleted = p.IsCompleted
    //     }));
    // }

    // [HttpGet("{id}")]
    // public async Task<ActionResult> UpdateStep(int appointmentServiceId, int id, UpdateProcedureStatusDto dto)
    // {
    //     var procedure = await unit.Repository<ServiceProcedure>().GetByIdAsync(id);
    //     if (procedure == null || procedure.AppointmentServiceId != appointmentServiceId) return NotFound();

    //     procedure.IsCompleted = dto.IsCompleted;
    //     await unit.Complete();

    //     return NoContent();
    // }

    [HttpPut("{appointmentId}/procedure/{procedureId}")]
    public async Task<ActionResult> UpdateProcedureStep(int appointmentId, int procedureId, [FromBody] UpdateProcedureStatusDto dto)
    {
        var procedure = await unit.Repository<ServiceProcedure>().GetByIdAsync(procedureId);
        if (procedure == null || procedure.AppointmentId != appointmentId) return NotFound();

        procedure.IsCompleted = dto.IsCompleted;
        await unit.Complete();

        return NoContent();
    }

    [HttpGet("{appointmentId}/procedures")]
    public async Task<ActionResult<IReadOnlyList<ServiceProcedureDto>>> GetProcedures(int appointmentId)
    {
        var procedures = await unit.Repository<ServiceProcedure>()
            .ListAsync(new SpecificationByForeignKey<ServiceProcedure>(appointmentId, "AppointmentId"));

        return Ok(procedures.Select(p => new ServiceProcedure
        {
            Id = p.Id,
            StepName = p.StepName,
            IsCompleted = p.IsCompleted
        }));
    }
}
