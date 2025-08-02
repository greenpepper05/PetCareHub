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
            Role = roles.FirstOrDefault()!
        });

    }

    // REGISTER NEW USER

    [HttpPost("register")]
    public async Task<ActionResult> Register(RegisterDto registerDto)
    {
        var user = new AppUser
        {
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName,
            Email = registerDto.Email,
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

    // TO BE UPDATE LATER

    // [Authorize]
    // public async Task<ActionResult<Address>> CreateOrUpdateAddress(AddressDto addressDto)
    // {
    //     var user = await signInManager.UserManager.GetUserByEmail(User);

    //     if (user.Address == null)
    //     {
    //         user.Address = addressDto.ToEntity();
    //     }
    //     else
    //     {
    //         user.Address.UpdateFromDto(addressDto);
    //     }

    //     var result = await signInManager.UserManager.UpdateAsync(user);

    //     if (!result.Succeeded) return BadRequest("Problem updating user address");

    //     return Ok(user.Address.ToDto());
    // }
}
