using API.DTOs;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class PetServiceHistoryController(IUnitOfWork unit, IMapper mapper) : BaseApiController
{

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PetServiceHistory>> GetHistoryByPetId(int id)
    {
        var spec = new PetServiceHistorySpec(id);
        var histories = await unit.Repository<PetServiceHistory>().GetEntityWithSpec(spec);

        return Ok(mapper.Map<PetServiceHistoryDto>(histories));

    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<PetServiceHistoryDto>>> GetAll()
    {
        var spec = new PetServiceHistorySpec();
        var histories = await unit.Repository<PetServiceHistory>().ListAsync(spec);
        return Ok(mapper.Map<IReadOnlyList<PetServiceHistoryDto>>(histories));
    }

    [HttpPost]
    public async Task<ActionResult> CreateHistory([FromBody] CreatePetServiceHistoryDto dto)
    {
        try
        {
            var history = mapper.Map<PetServiceHistory>(dto);
            unit.Repository<PetServiceHistory>().Add(history);
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
