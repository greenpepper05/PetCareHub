using API.DTOs;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class PetServiceHistoryController(IUnitOfWork unit, IMapper mapper) : BaseApiController
{
    [HttpGet("{petId}")]
    public async Task<ActionResult<IReadOnlyList<PetServiceHistoryDto>>> GetHistoryByPetId(int petId)
    {
        var spec = new PetServiceHistorySpec(petId);
        var histories = await unit.Repository<PetServiceHistory>().ListAsync(spec);
        return Ok(mapper.Map<IReadOnlyList<PetServiceHistoryDto>>(histories));
        
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<PetServiceHistoryDto>>> GetAll()
    {
        var spec = new PetServiceHistorySpec();
        var histories = await unit.Repository<PetServiceHistory>().ListAsync(spec);
        return Ok(mapper.Map<IReadOnlyList<PetServiceHistoryDto>>(histories));
    }

    [HttpPost]
    public async Task<ActionResult> CreateHistory([FromBody]CreatePetServiceHistoryDto dto)
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
