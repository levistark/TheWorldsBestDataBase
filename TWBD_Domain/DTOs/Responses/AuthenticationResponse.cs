using TWBD_Domain.DTOs.Enums;
using TWBD_Domain.Interfaces;

namespace TWBD_Domain.DTOs.Responses;
public class AuthenticationResponse : IServiceResponse
{
    public bool Success { get; set; } = false;
    public string? Message { get; set; }
    public AuthenticationCode Code { get; set; }
}
