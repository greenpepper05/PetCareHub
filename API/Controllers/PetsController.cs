using API.DTOs;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class PetsController(UserManager<AppUser> userManager, IUnitOfWork unit) : BaseApiController
{

    // CREATE PET PROFILE WITH OWNER ID

    [HttpPost]
    public async Task<ActionResult> AddPet(CreatePetDto createPetDto)
    {

        var owner = await userManager.FindByIdAsync(createPetDto.OwnerId);
        if (owner == null) return NotFound("User not found");

        var pet = new Pet
        {
            Name = createPetDto.Name,
            Breed = createPetDto.Breed,
            Species = createPetDto.Species,
            Birthdate = createPetDto.Birthdate,
            Gender = createPetDto.Gender,
            OwnerId = createPetDto.OwnerId
        };

        unit.Repository<Pet>().Add(pet);

        if (await unit.Complete())
        {
            var petDto = new PetDto
            {
                Id = pet.Id,
                Name = pet.Name,
                Breed = pet.Breed,
                Species = pet.Species,
                Birthdate = pet.Birthdate,
                Gender = pet.Gender,
                OwnerId = pet.OwnerId
            };

            return CreatedAtAction("GetPetsByOwner", new { id = pet.Id }, petDto);
        }

        return NoContent();
    }

    // SELECT PET BY ID

    [HttpGet("{id:int}")]
    public async Task<ActionResult> GetPetById(int id)
    {
        var pet = unit.Repository<Pet>().GetByIdAsync(id);

        return Ok(await pet);
    }

    // SELECT PETS BY CLINIC
    // [Authorize(Roles = "Admin")]
    // [HttpGet("clinic")]
    // public async Task<ActionResult<IReadOnlyList<Pet>>> GetPetsByClinic()
    // {
    //     var user = await userManager.GetUserAsync(User);

    //     if (user == null) return NotFound("No user found!");

    //     var spec = new PetByClinicIdSpecification(user.ClinicId);
    //     var pets = await unit.Repository<Pet>().ListAsync(spec);

    //     return Ok(pets);
    // }

    // SELECT PETS BY OWNER

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Pet>>> GetPetsByOwner()
    {
        var user = await userManager.GetUserAsync(User);
        if (user == null) return NotFound("No user found");

        var spec = new PetByOwnerIdSpec(user.Id.ToString());

        var pets = await unit.Repository<Pet>().ListAsync(spec);

        var result = pets.Select(p => new PetDto
        {
            Id = p.Id,
            Name = p.Name,
            Breed = p.Breed,
            Species = p.Species,
            Birthdate = p.Birthdate,
            Gender = p.Gender
        }).ToList();

        return Ok(result);
    }

    [HttpGet("all")]
    public async Task<ActionResult<IReadOnlyList<Pet>>> GetPets()
    {
        var pet = unit.Repository<Pet>().ListAllAsync();

        return Ok(await pet);
    }
}   
