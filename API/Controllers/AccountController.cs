using System.Security.Claims;
using API.DTOs;
using API.Extensions;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;


public class AccountController(SignInManager<AppUser> signInManager,
    UserManager<AppUser> userManager, IUnitOfWork unit, IOtpService otpService) : BaseApiController
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

    [HttpPost("send-otp")]
    public async Task<ActionResult> SendOtp([FromBody] OtpSendDto otpSendDto)
    {
        if (string.IsNullOrEmpty(otpSendDto.Email)) return BadRequest(new { message = "Email is required." });

        var user = await userManager.FindByEmailAsync(otpSendDto.Email);
        if (user != null) return BadRequest(new { message = "An account with this email already exists." });

        try
        {
            await otpService.GenerateAndSendOtpAsync(otpSendDto.Email);

            return Ok(new { message = "Verification code sent successfully." });
        }
        catch(Exception)
        {
            return StatusCode(500, new { message = "Error sending verification code. Please try again." });
        }
    }

    // REGISTER NEW USER

    [HttpPost("register")]
    public async Task<ActionResult> Register([FromBody] RegisterDto registerDto)
    {
        var isOtpValid = await otpService.ValidateOtpAsync(registerDto.Email, registerDto.Otp);
        if (!isOtpValid) return BadRequest(new { message = "Invalid or expired verification code." });

        var user = new AppUser
        {
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName,
            Email = registerDto.Email,
            Contact = registerDto.Contact,
            UserName = registerDto.Email,
            ClinicId = registerDto.ClinicId,
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(user, registerDto.Password);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }

            return ValidationProblem();
        }

        await userManager.AddToRoleAsync(user, "Customer");

        await otpService.InvalidateOtpAsync(registerDto.Email);

        return Ok(new { message = "Registration successful" });
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

    [Authorize(Roles = "Admin, SuperAdmin")]
    [HttpGet("users/{id}")]
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
    public async Task<ActionResult> AssignClinicToAdmin(string userId, [FromBody] AssignClinicDto clinicId)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user == null) return NotFound("User not found");

        // var clinic = await unit.Repository<Clinic>().GetByIdAsync(clinicId);
        // if (clinic == null) return NotFound("Clinic not found");

        user.ClinicId = clinicId.ClinicId;

        var result = await userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            return BadRequest("Failed to update user.");
        }

        return Ok(new { message = $"User {user.Email} successfully assigned to clinic" });
    }

    [Authorize(Roles = "SuperAdmin")]
    [HttpGet("admin-users")]
    public async Task<ActionResult<IReadOnlyList<UserDto>>> GetAllAdminUsers()
    {
        var appUsers = await userManager.Users
            .Include(u => u.Clinic)
            .ToListAsync();

        if (appUsers == null || appUsers.Count == 0) return NotFound("No users found");

        var userDtos = new List<UserDto>();

        foreach (var user in appUsers)
        {
            var roles = await userManager.GetRolesAsync(user);

            var userDto = new UserDto
            {
                Id = user.Id,
                Email = user.Email!,
                FirstName = user.FirstName!,
                LastName = user.LastName!,
                Contact = user.Contact!,
                Role = roles.FirstOrDefault()!,

                ClinicId = user.Clinic?.Id,
                ClinicName = user.Clinic?.ClinicName
            };

            if (userDto.Role == "Admin")
            {
                userDtos.Add(userDto);
            }
        }

        return Ok(userDtos);

    }

    [Authorize(Roles = "SuperAdmin")]
    [HttpGet("users")]
    public async Task<ActionResult<IReadOnlyList<UserDto>>> GetAllUsers()
    {
        var appUsers = await userManager.Users
            .Include(u => u.Clinic)
            .ToListAsync();

        if (appUsers == null || appUsers.Count == 0) return NotFound("No users found");

        var userDtos = new List<UserDto>();

        foreach (var user in appUsers)
        {
            var roles = await userManager.GetRolesAsync(user);

            var userDto = new UserDto
            {
                Id = user.Id,
                Email = user.Email!,
                FirstName = user.FirstName!,
                LastName = user.LastName!,
                Contact = user.Contact!,
                Role = roles.FirstOrDefault()!,

                ClinicId = user.Clinic?.Id,
                ClinicName = user.Clinic?.ClinicName
            };

            userDtos.Add(userDto);
        }

        return Ok(userDtos);

    }

    // Register AdminUser
    [Authorize(Roles = "SuperAdmin")]
    [HttpPost("register/admin")]
    public async Task<ActionResult> RegisterAdmin([FromBody] RegisterDto registerDto)
    {
        const string defaultPassword = "Pa$$w0rd";

        var user = new AppUser
        {
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName,
            Email = registerDto.Email,
            Contact = registerDto.Contact,
            UserName = registerDto.Email,
            ClinicId = registerDto.ClinicId,
        };

        var result = await signInManager.UserManager.CreateAsync(user, defaultPassword);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }

            return ValidationProblem();
        }

        await userManager.AddToRoleAsync(user, "Admin");

        return Ok();
    }

    [Authorize(Roles = "SuperAdmin")]
    [HttpGet("user/{id}")]
    public async Task<ActionResult<UserDto>> GetUserWithClinicById(string id)
    {
        var user = await userManager.Users.Include(u => u.Clinic).Where(u => u.Id == id).FirstOrDefaultAsync();

        if (user == null) return NotFound();

        var roles = await userManager.GetRolesAsync(user);

        return Ok(new UserDto
        {
            Id = user.Id,
            Email = user.Email!,
            FirstName = user.FirstName!,
            LastName = user.LastName!,
            Contact = user.Contact!,
            ClinicId = user.Clinic?.Id,
            ClinicName = user.Clinic?.ClinicName,
            Role = roles.FirstOrDefault()
        });
    }

    [Authorize(Roles = "SuperAdmin")]
    [HttpDelete("remove-user/{id}")]
    public async Task<ActionResult> RemoveUser(string id)
    {
        var user = await userManager.FindByIdAsync(id);

        if (user == null) return NotFound($"User with ID ${id} not found");

        var roles = await userManager.GetRolesAsync(user);
        if (roles.Contains("SuperAdmin"))
        {
            return BadRequest("Cannot delete SuperAdmin accounts through thi enpoint");
        }

        var result = await userManager.DeleteAsync(user);

        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description);
            return BadRequest($"Failed to delete user: {string.Join(", ", errors)}");
        }
        return NoContent();
    }

}
