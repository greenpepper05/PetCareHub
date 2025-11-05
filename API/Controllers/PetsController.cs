using System.Linq.Expressions;
using API.DTOs;
using API.Extensions;
using API.RequestHelpters;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class PetsController(UserManager<AppUser> userManager, IUnitOfWork unit, IMapper mapper, IWebHostEnvironment env) : BaseApiController
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
            OwnerId = createPetDto.OwnerId,
            PictureUrl = createPetDto.PictureUrl
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
                OwnerId = pet.OwnerId,
                PictureUrl = pet.PictureUrl!
            };

            return CreatedAtAction("GetPetsByOwner", new { id = pet.Id }, petDto);
        }

        return NoContent();
    }

    [HttpPost("upload/pet")]
    public async Task<ActionResult<ImageUploadResponseDto>> UploadPetImage([FromForm] IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest(new { message = "No file uploaded" });
        }

        var uploadPath = Path.Combine(
            env.WebRootPath,
            "assets",
            "images",
            "pets"
        );

        if (!Directory.Exists(uploadPath))
        {
            Directory.CreateDirectory(uploadPath);
        }

        var fileExtension = Path.GetExtension(file.FileName);

        var uniqueFileName = $"{Guid.NewGuid().ToString()}{fileExtension}";
        var filePath = Path.Combine(uploadPath, uniqueFileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        var publicUrl = $"/assets/images/pets/{uniqueFileName}";

        return Ok(new ImageUploadResponseDto { Url = publicUrl });
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
            Gender = p.Gender,
            PictureUrl = p.PictureUrl!
        }).ToList();

        return Ok(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("{clinicId:int}/paginated")]
    public async Task<ActionResult<Pagination<PetDto>>> GetPaginatedPet([FromQuery] PetSpecParams specParams, int clinicId)
    {

        var spec = new PetsByClinicIdSpecification(specParams, clinicId);

        var serviceRecords = await unit.Repository<Appointment>().ListAsync(spec);

        var pets = serviceRecords
            .Where(sr => sr.Pet != null)
            .Select(sr => sr.Pet!)
            .DistinctBy(p => p.Id)
            .ToList();

        var totalCount = pets.Count;

        var data = mapper.Map<IReadOnlyList<PetDto>>(pets);

        return Ok(new Pagination<PetDto>(
            specParams.PageIndex,
            specParams.PageSize,
            totalCount,
            data
        ));
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("{clinicId:int}/all-pets")]
    public async Task<ActionResult<IReadOnlyList<PetDto>>> GetPets(int clinicId)
    {
        var spec = new PetByClinicIdSpecification(clinicId);
        var serviceRecord = await unit.Repository<Appointment>().ListAsync(spec);

        var pets = serviceRecord
            .Where(sr => sr.Pet != null)
            .Select(sr => sr.Pet!)
            .DistinctBy(p => p.Id)
            .ToList();

        var data = mapper.Map<IReadOnlyList<PetDto>>(pets);

        return Ok(data);

    }
    
    [Authorize(Roles = "SuperAdmin")]
    [HttpGet("all")]
    public async Task<ActionResult<IReadOnlyList<PetDto>>> GetAllPets()
    {
        var user = await userManager.GetUserByEmail(User);

        if (user == null) return Unauthorized();

        var pets = await unit.Repository<Pet>().ListAllAsync();

        var mapped = mapper.Map<IReadOnlyList<PetDto>>(pets);

        return Ok(mapped);
    }

}   
