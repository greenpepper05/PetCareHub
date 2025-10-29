using API.DTOs;
using API.Extensions;
using API.RequestHelpters;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class ServiceRecordController(IUnitOfWork unit,
    IMapper mapper, UserManager<AppUser> userManager, IEmailService emailService) : BaseApiController
{
    // GET SERVICE RECORD BY ID
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ServiceRecord>> GetServiceRecordById(int id)
    {
        var spec = new ServiceRecordSpecification(id);

        var record = await unit.Repository<ServiceRecord>().GetEntityWithSpec(spec);

        return Ok(mapper.Map<ServiceRecordDto>(record));
    }

    // GET ALL SERVICE RECORD BY CLINIC ID

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

    // CREATE SERVICE RECORD

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<ServiceRecordDto>> CreateServiceRecord([FromBody] CreateServiceRecordDto dto)
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
    [HttpGet("pet")]
    public async Task<ActionResult<IReadOnlyList<ServiceRecordDto>>> GetServiceRecordByPetAndClinic(
        [FromQuery] ServiceRecordSpecParams specParams,
        [FromQuery] int petId, [FromQuery] int clinicId)
    {
        if (petId <= 0)
        {
            return BadRequest("A valid PetId must be proviced");
        }

        if (clinicId <= 0)
        {
            return BadRequest("A valid ClinicId must be proviced");
        }

        var spec = new ServiceRecordByPetIdAndClinicIdSpecification(specParams, petId, clinicId);
        var countSpec = new ServiceRecordByPetIdAndClinicIdSpecification(specParams, petId, clinicId);

        var totalItems = await unit.Repository<ServiceRecord>().CountAsync(countSpec);

        var serviceRecords = await unit.Repository<ServiceRecord>().ListAsync(spec);

        if (serviceRecords == null || serviceRecords.Count == 0)
        {
            return Ok(new List<ServiceRecord>());
        }

        var data = mapper.Map<IReadOnlyList<ServiceRecord>, IReadOnlyList<ServiceRecordDto>>(serviceRecords);

        return Ok(new Pagination<ServiceRecordDto>(
            specParams.PageIndex,
            specParams.PageSize,
            totalItems,
            data
        ));
    }


    [Authorize(Roles = "Admin")]
    [HttpGet("records/{recordId}/steps")]
    public async Task<ActionResult<IReadOnlyList<ServiceRecordStepDto>>> GetSteps(int recordId)
    {
        var spec = new ServiceRecordStepsSpecification(recordId);
        var record = await unit.Repository<ServiceRecordStep>().ListAsync(spec);

        if (record == null) return Ok(new List<ServiceRecordStepDto>());

        var data = mapper.Map<IReadOnlyList<ServiceRecordStepDto>>(record);

        return Ok(data);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("records/{recordId}/steps/{stepId}/complete")]
    public async Task<ActionResult> CompleteStep(int recordId, int stepId)
    {
        var step = await unit.Repository<ServiceRecordStep>().GetByIdAsync(stepId);
        if (step == null || step.ServiceRecordId != recordId) return NotFound();

        var phTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Singapore Standard Time");
        var nowPST = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, phTimeZone);

        step.IsCompleted = true;
        step.CompletedAt = nowPST;

        unit.Repository<ServiceRecordStep>().Update(step);
        await unit.Complete();

        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpPatch("records/{recordId}/steps/{stepId}/skip")]
    public async Task<ActionResult> SkipStep(int recordId, int stepId, [FromBody] SkipDto dto)
    {
        var step = await unit.Repository<ServiceRecordStep>().GetByIdAsync(stepId);
        if (step == null || step.ServiceRecordId != recordId) return NotFound();

        step.IsSkipped = dto.IsSkipped;

        unit.Repository<ServiceRecordStep>().Update(step);
        await unit.Complete();

        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpPatch("records/{serviceId}")]
    public async Task<ActionResult> CompleteProcedure(int serviceId)
    {
        var serviceRecord = await unit.Repository<ServiceRecord>().GetByIdAsync(serviceId);

        var spec = new PetServiceIdSpecification(serviceId);
        var pet = await unit.Repository<Pet>().GetEntityWithSpec(spec);

        if (serviceRecord == null) return NotFound();
        if (pet == null) return NotFound();

        serviceRecord.Status = "Completed";

        unit.Repository<ServiceRecord>().Update(serviceRecord);
        await unit.Complete();

        var subject = "Pet Service Completed";
        var message = $"Hello <strong>{pet.Owner.FirstName} {pet.Owner.LastName}</strong>, Your pet <strong>{pet.Name}</strong> service has been <strong>Completed</strong> and ready for pick-up .<br><br>Thank you!";
        var email = pet.Owner.Email;

        if (!string.IsNullOrWhiteSpace(email))
        {
            await emailService.SendEmailAsync(email, subject, message);
        }

        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("records/{serviceId}")]
    public async Task<ActionResult> DeleteService(int serviceId)
    {
        var user = await userManager.GetUserByEmail(User);
        if (user == null) return Unauthorized();

        var serviceRecord = await unit.Repository<ServiceRecord>().GetByIdAsync(serviceId);
        if (serviceRecord == null) return NotFound();

        unit.Repository<ServiceRecord>().Remove(serviceRecord);

        await unit.Complete();

        return Ok("Record succesfully removed!");
    } 

    [Authorize(Roles = "Admin")]
    [HttpGet("all")]
    public async Task<ActionResult<IReadOnlyList<ServiceRecordDto>>> GetAllRecords()
    {
        var user = await userManager.GetUserByEmail(User);

        if (user == null) return Unauthorized();

        var serviceRecord = await unit.Repository<ServiceRecord>().ListAllAsync();

        if (serviceRecord == null) return NotFound();

        var srDto = mapper.Map<IReadOnlyList<ServiceRecordDto>>(serviceRecord);

        return Ok(srDto);
    }
}
