using System.Security.Claims;
using API.DTOs;
using API.Extensions;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;


public class AccountController(SignInManager<AppUser> signInManager,
    UserManager<AppUser> userManager, IUnitOfWork unit) : BaseApiController
{

    // GET USER INFRO

    [HttpGet("user-info")]
    public async Task<ActionResult> GetUserInfo()
    {
        if (User.Identity?.IsAuthenticated == false) return NoContent();

        var user = await signInManager.UserManager.GetUserByEmail(User);
        var roles = await userManager.GetRolesAsync(user);

        return Ok(new UserInfoDto
        {
            Id = user.Id,
            Email = user.Email!,
            FirstName = user.FirstName!,
            LastName = user.LastName!,
            Contact = user.Contact!,
            Role = roles.FirstOrDefault()!,
            ClinicId = user.ClinicId!
        });

    }

    // REGISTER NEW USER

    [HttpPost("register")]
    public async Task<ActionResult> Register([FromBody] RegisterDto registerDto)
    {
        var user = new AppUser
        {
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName,
            Email = registerDto.Email,
            Contact = registerDto.Contact,
            UserName = registerDto.Email,
            ClinicId = registerDto.ClinicId
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

        await userManager.AddToRoleAsync(user, "Customer");

        return Ok();
    }

    // LOGOUT USER

    [Authorize]
    [HttpPost("logout")]
    public async Task<ActionResult> Logout()
    {
        await signInManager.SignOutAsync();

        return NoContent();
    }

    // GET AUTHENTICATION STATUS
    [HttpGet("auth-status")]
    public ActionResult GetAuthState()
    {
        return Ok(new { IsAuthenticated = User.Identity?.IsAuthenticated ?? false });
    }

    // GET ALL PETS BY THE LOGGED-IN USER

    [Authorize]
    [HttpGet("my-pets")]
    public async Task<ActionResult<IReadOnlyList<Pet>>> GetMyPets()
    {
        var user = await userManager.GetUserAsync(User);

        if (user == null) return Unauthorized("User not found");

        var pets = await unit.Repository<Pet>().ListAsync(new BaseSpecification<Pet>(p => p.OwnerId == user.Id));

        return Ok(pets);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("user/{id}")]
    public async Task<ActionResult<UserDto>> GetUserById(string id)
    {
        var user = await userManager.FindByIdAsync(id);

        if (user == null) return NotFound("User not found");

        var roles = await userManager.GetRolesAsync(user);

        return Ok(new UserDto
        {
            Id = user.Id,
            Email = user.Email!,
            FirstName = user.FirstName!,
            LastName = user.LastName!,
            Contact = user.Contact!,
            Role = roles.FirstOrDefault() ?? "Customer"
        });

    }

    [Authorize]
    [HttpPut("assign-clinic/{userId}")]
    public async Task<ActionResult> AssignClinicToAdmin(string userId, [FromQuery] int clinicId)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user == null) return NotFound("User not found");

        var clinic = await unit.Repository<Clinic>().GetByIdAsync(clinicId);
        if (clinic == null) return NotFound("Clinic not found");

        user.ClinicId = clinicId;

        var result = await userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            return BadRequest("Failed to update user.");
        }

        return Ok(new { message = $"User {user.Email} successfully assigned to clinic {clinic.ClinicName}" });
    }

}
