using System;
using API.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class ServiceRecordController : BaseApiController
{
    [HttpPost]
    public async Task<ActionResult> CreateServiceRecord([FromBody] CreateServiceRecordDto dto)
    {
        
        return await Ok(a);
    }
}
