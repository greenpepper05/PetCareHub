using System;
using API.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class FileUploadController(IWebHostEnvironment env) : BaseApiController
{
    [HttpPost]
    public async Task<ActionResult<ImageUploadResponseDto>> Upload([FromForm] IFormFile file)
    {
        if (file == null)
        {
            return BadRequest("File not found");
        }

        try
        {
            string uploadPath = Path.Combine(
                env.WebRootPath,
                "assets"
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
        catch (System.Exception)
        {
            
            throw;
        }
    }
}
