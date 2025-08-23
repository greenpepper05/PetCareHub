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

            return Ok();
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

}
