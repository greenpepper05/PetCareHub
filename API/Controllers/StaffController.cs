using API.DTOs;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class StaffController(IUnitOfWork unit, IMapper mapper, IWebHostEnvironment env) : BaseApiController
{
   [HttpGet]
   public async Task<ActionResult<IReadOnlyList<StaffDto>>> GetStaff(int clinicId)
    {
        var spec = new StaffByClinicSpecification(clinicId);

        var staff = await unit.Repository<Staff>().ListAsync(spec);

        return Ok(mapper.Map<IReadOnlyList<StaffDto>>(staff));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<StaffDto>> GetStaffById(int id, int clinicId)
    {
        var spec = new StaffByClinicSpecification(id, clinicId);

        var staff = await unit.Repository<Staff>().GetEntityWithSpec(spec);

        if (staff == null) return NotFound("Staff not found");

        return Ok(mapper.Map<StaffDto>(staff));
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<StaffDto>> CreateStaff([FromBody] CreateStaffDto dto)
    {
        var staff = new Staff
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            StaffRole = dto.StaffRole,
            PictureUrl = dto.PictureUrl,
            ClinicId = dto.ClinicId
        };

        unit.Repository<Staff>().Add(staff);

        if (!await unit.Complete()) return BadRequest("Failed to create staff");

        
        var spec = new StaffByClinicSpecification(staff.Id, dto.ClinicId);
        var created = await unit.Repository<Staff>().GetEntityWithSpec(spec);

        return Ok(mapper.Map<StaffDto>(created));
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("upload")]
    public async Task<ActionResult<ImageUploadResponseDto>> UploadStaffImage([FromForm] IFormFile file)
    {
        if (file == null || file.Length == 0) return BadRequest(new { message = "No file uploaded"});

        var uploadPath = Path.Combine(
            env.WebRootPath,
            "assets",
            "images",
            "profiles"
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

        var publicUrl = $"/assets/images/profiles/{uniqueFileName}";

        return Ok(new ImageUploadResponseDto { Url = publicUrl});
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("delete/{id}")]
    public async Task<ActionResult> DeleteStaff(int id)
    {
        var staff = await unit.Repository<Staff>().GetByIdAsync(id);

        if (staff == null) return NotFound();

        unit.Repository<Staff>().Remove(staff);

        if (await unit.Complete()) return NoContent();

        return BadRequest("Failed to delete staff");
    }
}
