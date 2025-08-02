using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;


public class ClinicController(IUnitOfWork unit) : BaseApiController
{

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Clinic>>> GetClinic()
    {
        var clinic = await unit.Repository<Clinic>().ListAllAsync();

        return Ok(clinic);
    }

}
