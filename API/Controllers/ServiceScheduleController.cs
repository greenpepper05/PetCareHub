using API.DTOs;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class ServiceScheduleController(IUnitOfWork unit,
    UserManager<AppUser> userManager) : BaseApiController
{

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ServiceScheduleDto>>> GetAllSchedule()
    {
        var schedules = await unit.Repository<ServiceSchedule>().ListAllAsync();
        return Ok(schedules);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ServiceScheduleDto>> GetById(int id)
    {
        var schedule = await unit.Repository<ServiceSchedule>().GetByIdAsync(id);
        if (schedule == null) return NotFound();

        return Ok(schedule);
    }

    [HttpPost]
    public async Task<ActionResult<ServiceScheduleDto>> CreateSchedule(CreateServiceScheduleDto dto)
    {
        var schedule = new ServiceSchedule
        {
            ServiceName = dto.ServiceName,
            SessionCount = dto.SessionCount,
            IntervalInDays = dto.IntervalInDays,
            ServiceId = dto.ServiceId
        };

        unit.Repository<ServiceSchedule>().Add(schedule);

        if (await unit.Complete())
        {
            var result = new ServiceSchedule
            {
                ServiceName = schedule.ServiceName,
                SessionCount = schedule.SessionCount,
                IntervalInDays = schedule.IntervalInDays,
                ServiceId = schedule.ServiceId
            };
            return CreatedAtAction("GetById", new { id = schedule.Id }, result);
        }

        return BadRequest("Problem Creating schedule");
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateSchedule(int id, ServiceScheduleDto dto)
    {
        var existing = await unit.Repository<ServiceSchedule>().GetByIdAsync(id);
        if (existing == null) return NotFound();

        var schedule = new ServiceSchedule
        {
            ServiceName = dto.ServiceName,
            SessionCount = dto.SessionCount,
            IntervalInDays = dto.IntervalInDays,
            ServiceId = dto.ServiceId
        };

        unit.Repository<ServiceSchedule>().Update(schedule);

        if (await unit.Complete()) return NoContent();

        return BadRequest("Problem creating schedule");
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteSchedule(int id)
    {
        var schedule = await unit.Repository<ServiceSchedule>().GetByIdAsync(id);
        if (schedule == null) return NotFound("Schedule no found");

        unit.Repository<ServiceSchedule>().Remove(schedule);

        if (await unit.Complete()) return NoContent();

        return BadRequest("Problem deleting service");
    }

}
