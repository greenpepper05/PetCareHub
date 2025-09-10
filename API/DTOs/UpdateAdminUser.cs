using System;

namespace API.DTOs;

public class UpdateAdminUser
{
    public string Id { get; set; } = string.Empty;
    public int ClinicId { get; set; }
}
