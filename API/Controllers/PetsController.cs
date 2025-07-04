using API.DTOs;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize(Roles = "Admin")]
public class PetsController(UserManager<AppUser> userManager, IUnitOfWork unit) : BaseApiController
{
    [HttpPost]
    public async Task<ActionResult> AddPet(CreatePetDto createPetDto)
    {
        var user = await userManager.GetUserAsync(User);

        if (user != null)
        {
            var pet = new Pet
            {
                Name = createPetDto.Name,
                Breed = createPetDto.Breed,
                Species = createPetDto.Species,
                Birthdate = createPetDto.Birthdate,
                Gender = createPetDto.Gender,
                OwnerId = user.Id
            };

            unit.Repository<Pet>().Add(pet);

            if (await unit.Complete())
            {
                return CreatedAtAction("GetPetById", new { id = pet.Id }, pet);
            }
        }

        return NoContent();
        
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult> GetPetById(int id)
    {
        var pet = unit.Repository<Pet>().GetByIdAsync(id);

        return Ok(await pet);
    }
}
