namespace TWBD_Domain.DTOs.Responses;
public class AuthenticationResponse
{
    public bool Success = false;
    public string? Message { get; set; }
    public AuthenticationCode Code { get; set; }
}
