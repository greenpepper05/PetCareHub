using System.Security.Claims;
using API.DTOs;
using API.Extensions;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize(Roles = "SuperAdmin")]
public class SuperAdminController(SignInManager<AppUser> signInManager,
    UserManager<AppUser> userManager, IUnitOfWork unit, IWebHostEnvironment env) 
    : BaseApiController
{
    
    [HttpGet("user-info")]
    public async Task<ActionResult> GetUserInfo()
    {
        if (User.Identity?.IsAuthenticated == false) return NoContent();

        var user = await signInManager.UserManager.GetUserByEmail(User);

        return Ok(new
        {
            user.FirstName,
            user.LastName,
            user.Email,
            Roles = User.FindFirstValue(ClaimTypes.Role)
        });
    }

    [HttpPost("register")]
    public async Task<ActionResult> RegisterUser(RegisterDto registerDto)
    {
        var exisingUser = await userManager.FindByEmailAsync(registerDto.Email);

        if (exisingUser != null)
        {
            return BadRequest("Email is already registered");
        }

        var user = new AppUser
        {
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName,
            Email = registerDto.Email,
            UserName = registerDto.Email,
        };

        var result = await signInManager.UserManager.CreateAsync(user, registerDto.Password);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }

            return ValidationProblem();
        }

        await userManager.AddToRoleAsync(user, "Admin");

        return Ok(new
        {
            user.Id,
            user.Email,
            user.FirstName,
            Role = "Admin",
        });
    }

    // REGISTER CLINIC

    [HttpPost("register-clinic")]
    public async Task<ActionResult> RegisterClinic(RegisterClinicDto clinicDto)
    {
        var user = await userManager.FindByIdAsync(clinicDto.OwnerId);
        if (user == null) return NotFound("No user found");

        var clinic = new Clinic
        {
            OwnerId = clinicDto.OwnerId,
            ClinicName = clinicDto.ClinicName,
            PhoneNumber = clinicDto.PhoneNumber,
            Address = clinicDto.Address,
            Email = clinicDto.Email,
            PictureUrl = clinicDto.PictureUrl

        };

        unit.Repository<Clinic>().Add(clinic);

        if (await unit.Complete()) return NoContent();

        return BadRequest("Error Creating Clinic");
    }

    // Update Admin

    [HttpPost("update-admin/{id}")]
    public async Task<ActionResult> UpdateAdminUser(UpdateAdminUser dto, string id)
    {
        var user = await userManager.FindByIdAsync(id);
        if (user == null) return NotFound("No user found");

        user.ClinicId = dto.ClinicId;

        var result = await userManager.UpdateAsync(user);

        if (result.Succeeded) return NoContent();

        return BadRequest("Error Updating Admin User");
    }

    [HttpPost("upload")]
    public async Task<ActionResult<ImageUploadResponseDto>> Upload([FromForm] IFormFile file)
    {
        if (file == null || file.Length == 0) return BadRequest("File not found");
        
        try
        {
            string uploadPath = Path.Combine(
                env.ContentRootPath,
                "..",
                "client",
                "public",
                "assets",
                "images",
                "clinic-logo"
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

            var publicUrl = $"/assets/images/clinic-logo/{uniqueFileName}";

            return Ok(new ImageUploadResponseDto { Url = publicUrl });

        }
        catch (System.Exception)
        {
            throw;
        }
    }
}
