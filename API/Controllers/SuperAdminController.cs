using System;
using System.Security.Claims;
using API.DTOs;
using API.Extensions;
using Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize(Roles = "SuperAdmin")]
public class SuperAdminController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager) : BaseApiController
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
        
}
