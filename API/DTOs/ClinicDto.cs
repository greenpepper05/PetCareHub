using System;

namespace API.DTOs;

public class ClinicDto
{
    public int Id { get; set; }
    public string OwnerId { get; set; } = string.Empty;
    public string ClinicName { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PictureUrl { get; set; } = string.Empty;
    public string SignatureUrl { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public bool IsOpen { get; set; }
    public DateTime CreateAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
