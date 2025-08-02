using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class ServicesController(IUnitOfWork unit) : BaseApiController
{

    // GET ALL SERVICES

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Service>>> GetServices()
    {
        var services = await unit.Repository<Service>().ListAllAsync();
        return Ok(services);
    }

}
