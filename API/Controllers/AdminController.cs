using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController(IUnitOfWork unit) : BaseApiController
{

    // GET LIST OF ALL PETS

    // [HttpGet]
    // public async Task<ActionResult> GetAllPets()
    // {
    // }

    // ------------------------------------------

    // GET PETS BY ID

    // [HttpGet("{id:int}")]
    // public async Task<ActionResult<>> GetPetById(int id)
    // {

    // }
}
