namespace API.DTOs;

public class OtpSendDto
{
    public string Email { get; set; } = string.Empty;
    public int? ClinicId { get; set; }
}
